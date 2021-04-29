using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DataTransferObjects.Forums.Forum;
using Domain.DataTransferObjects.Forums.Forum.Input;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Enums.Neo;
using Domain.Exceptions;
using Domain.Services.Forum;
using Domain.Services.Repositories;
using Domain.Services.User;

namespace Services.Forum
{
    public class ForumService : IForumService
    {
        private readonly ISQLRepository<Domain.Entities.SQL.Forums.Forum> _forumSqlRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly INeoRepository<Domain.Entities.SQL.Forums.Forum> _forumNeoRepository;

        public ForumService(ISQLRepository<Domain.Entities.SQL.Forums.Forum> forumSqlRepository, IAuthenticationService authenticationService, IMapper mapper, INeoRepository<Domain.Entities.SQL.Forums.Forum> forumNeoRepository)
        {
            _forumSqlRepository = forumSqlRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _forumNeoRepository = forumNeoRepository;
        }
        
        /**
         * Creates a forum using an user EntityId
         * Returns the uploaded forum
         */
        public async Task<ForumDTO> CreateForum(CreateForumInput createForum, string token)
        {
            var userFromToken = await TokenHandler.TokenValidationAndReturnUser(this._authenticationService, token);
            // Add user to forum
            var forum = new Domain.Entities.SQL.Forums.Forum
            {
                Title = createForum.Title,
                CreatedByUser = userFromToken
            };

            // Inserts forum into database
            var insertedForum = await _forumSqlRepository.Insert(forum);

            // insert into graph database
            await _forumNeoRepository.Insert(insertedForum);

            // Insert relation into graph database
            await _forumNeoRepository.CreateRelation(userFromToken,
                NEORelation.CREATED, insertedForum);

            // returns the newly created forum
            return _mapper.Map<ForumDTO>(insertedForum);
        }
        
        /**
         * Updates a forum using forum EntityId
         * Returns updated forum
         */
        public async Task<ForumDTO> UpdateForum(Guid forumId, Domain.Entities.SQL.Forums.Forum forum, string token)
        {
            // Get user from token
            var userFromToken = await TokenHandler.TokenValidationAndReturnUser(this._authenticationService, token);

            // Find forum in database
            var foundForum = await _forumSqlRepository.Get(forumId);

            // Check if forum exists
            if (foundForum == null)
                // Throw an exception if forum has not been found
                throw new NotFoundException("Forum");

            // Set postId in post
            forum.Id = forumId;

            // Check if user is the creator of the forum
            if (foundForum.CreatedByUserId != userFromToken.Id) throw new NoPermissionException("edit");

            // Updates forum in database and returns the updated post
            return _mapper.Map<ForumDTO>(forum);
        }
        
        /**
         * Deletes forum by forum id
         * Returns deleted forum
         */
        public async Task<ForumDTO> DeleteForum(Guid postId, string token)
        {
            // Get user from token
            var userFromToken = await TokenHandler.TokenValidationAndReturnUser(_authenticationService, token);

            // Find forum in database
            var foundForum = await _forumSqlRepository.Get(postId);

            // Check if forum exists
            if (foundForum == null)
                // Throw an exception if forum has not been found
                throw new NotFoundException("Forum");

            // Check if user is the creator of the forum
            if (foundForum.CreatedByUser.Id != userFromToken.Id) throw new NoPermissionException("delete");

            // Deletes forum in database
            var deletedPost = await _forumSqlRepository.Delete(foundForum);

            //Deletes forum in graph database
            await _forumNeoRepository.Delete(deletedPost);

            // returns the deleted forum
            return _mapper.Map<ForumDTO>(deletedPost);
        }
        
        /**
         * Get all forums from database
         * Returns all forums from database
         */
        public async Task<IEnumerable<ForumDTO>> GetForums()
        {
            // Get all forums from database
            var foundPosts = await _forumSqlRepository.GetAll();

            // Returns found forums
            return _mapper.ProjectTo<ForumDTO>(foundPosts as IQueryable);
        }
    }
}
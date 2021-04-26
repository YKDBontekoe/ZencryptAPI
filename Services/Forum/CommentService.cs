using System;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.Entities.SQL.Forums;
using Domain.Enums.Neo;
using Domain.Exceptions;
using Domain.Services.Forum;
using Domain.Services.Repositories;
using Domain.Services.User;

namespace Services.Forum
{
    public class CommentService : ICommentService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISQLRepository<Comment> _commentIsqlRepository;
        private readonly IMapper _mapper;
        private readonly INeoRepository<Comment> _neoCommentRepository;
        private readonly INeoRepository<Post> _neoPostRepository;
        private readonly INeoRepository<Domain.Entities.SQL.User.User> _neoUserRepository;
        private readonly ISQLRepository<Post> _postIsqlRepository;

        public CommentService(IAuthenticationService authenticationService,
            ISQLRepository<Comment> commentIsqlRepository, INeoRepository<Comment> neoCommentRepository,
            INeoRepository<Post> neoPostRepository, INeoRepository<Domain.Entities.SQL.User.User> neoUserRepository,
            ISQLRepository<Post> postIsqlRepository, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _commentIsqlRepository = commentIsqlRepository;
            _neoCommentRepository = neoCommentRepository;
            _neoPostRepository = neoPostRepository;
            _neoUserRepository = neoUserRepository;
            _postIsqlRepository = postIsqlRepository;
            _mapper = mapper;
        }

        /**
         * Create a comment and add the new comment to an existing post
         * Returns the newly created comment
         */
        public async Task<CommentDTO> CreateCommentToPost(CreateCommentDTO comment)
        {
            // Get post from database by EntityId
            var foundPost = await _postIsqlRepository.Get(comment.PostId);

            // Check if post is in database
            if (foundPost == null)
                // Throw error if post is not found/ null
                throw new NotFoundException("Post");

            // Create Comment Object
            var dbComment = new Comment
            {
                Description = comment.Description,
                PostId = comment.PostId,
                UploadedUserId = comment.UserId
            };

            // Create new comment in sql database
            await _commentIsqlRepository.Insert(dbComment);
            await _neoPostRepository.CreateRelation(new Domain.Entities.SQL.User.User {Id = comment.UserId},
                NEORelation.COMMENTED, dbComment);
            await _neoPostRepository.CreateRelation(foundPost, NEORelation.COMMENT, dbComment);

            // Returns new comment
            return _mapper.Map<CommentDTO>(dbComment);
        }

        /**
         * Update a comment and check permission of comment
         * Returns updated comment
         */
        public async Task<CommentDTO> UpdateComment(Guid commentId, Comment comment, string token)
        {
            // Find comment in database
            var foundComment = await _commentIsqlRepository.Get(commentId);

            // Check if comment is in database
            if (foundComment == null)
                // Throw error if comment is not found/ null
                throw new NotFoundException("Comment");

            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
                // Throw an exception if token is invalid
                throw new InvalidTokenException();

            // Get user from token
            var userFromToken = await _authenticationService.GetUserFromToken(token);

            // Check if user is owner of comment
            if (foundComment.UploadedUserId != userFromToken.Id)
                // Throw an exception if comment is from user
                throw new NoPermissionException("Comment");

            // Update and return updated comment
            var dbComment = await _commentIsqlRepository.Update(comment);

            return _mapper.Map<CommentDTO>(dbComment);
        }

        /**
         * Deletes a single comment by id
         * Returns deleted comment
         */
        public async Task<CommentDTO> DeleteComment(Guid commentId, string token)
        {
            // Get comment from database
            var foundComment = await _commentIsqlRepository.Get(commentId);

            // Check if comment is in database
            if (foundComment == null)
                // Throw not found exception if comment is not in database
                throw new NotFoundException("comment");

            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
                // Throw an exception if token is invalid
                throw new InvalidTokenException();

            // Get user from token
            var userFromToken = await _authenticationService.GetUserFromToken(token);

            // Check if user is owner of comment
            if (foundComment.UploadedUserId != userFromToken.Id)
                // Throw an exception if comment is from user
                throw new NoPermissionException("Comment");

            // Delete comment and return deleted comment
            await _commentIsqlRepository.Delete(foundComment);
            await _neoCommentRepository.Delete(foundComment);
            return _mapper.Map<CommentDTO>(foundComment);
            ;
        }

        /**
         * Returns a single comment by id
         */
        public async Task<CommentDTO> GetComment(Guid commentId)
        {
            var comment = await _commentIsqlRepository.Get(commentId);
            return _mapper.Map<CommentDTO>(comment);
            ;
        }
    }
}
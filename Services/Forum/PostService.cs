using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Entities.SQL.Forums;
using Domain.Entities.SQL.User;
using Domain.Enums.Neo;
using Domain.Exceptions;
using Domain.Services.Forum;
using Domain.Services.Repositories;
using Domain.Services.User;

namespace Services.Forum
{
    public class PostService : IPostService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly INeoRepository<Post> _neoRepository;
        private readonly ISQLRepository<Post> _postIsqlRepository;
        private readonly ISQLRepository<UserDislikedPost> _userDislikedIsqlRepository;
        private readonly ISQLRepository<UserLikedPost> _userLikedIsqlRepository;
        private readonly ISQLRepository<Domain.Entities.SQL.User.User> _userSqlRepository;

        public PostService(IAuthenticationService authenticationService, INeoRepository<Post> neoRepository,
            ISQLRepository<Post> postIsqlRepository, ISQLRepository<UserDislikedPost> userDislikedIsqlRepository,
            ISQLRepository<UserLikedPost> userLikedIsqlRepository,
            ISQLRepository<Domain.Entities.SQL.User.User> userSqlRepository, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _neoRepository = neoRepository;
            _postIsqlRepository = postIsqlRepository;
            _userDislikedIsqlRepository = userDislikedIsqlRepository;
            _userLikedIsqlRepository = userLikedIsqlRepository;
            _userSqlRepository = userSqlRepository;
            _mapper = mapper;
        }

        /**
         * Creates a post using an user EntityId
         * Returns the uploaded post
         */
        public async Task<PostDTO> CreatePost(CreatePostDTO createPost, string token)
        {
            var userFromToken = await TokenValidation(token);
            // Add user to post
            var post = new Post
            {
                Title = createPost.Title,
                Description = createPost.Description,
                UploadedByUser = userFromToken
            };

            // Inserts post into database
            var insertedPost = await _postIsqlRepository.Insert(post);

            // insert into graph database
            await _neoRepository.Insert(insertedPost);
            
            // Insert relation into graph database
            await _neoRepository.CreateRelation(userFromToken,
                NEORelation.POSTED, insertedPost);

            // returns the newly created post
            return _mapper.Map<PostDTO>(insertedPost);
        }

        /**
         * Updates a post using post EntityId
         * Returns updated post
         */
        public async Task<PostDTO> UpdatePost(Guid postId, Post post, string token)
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);

            // Find post in database
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post exists
            if (foundPost == null)
                // Throw an exception if post has not been found
                throw new NotFoundException("Post");

            // Set postId in post
            post.Id = postId;

            // Check if user is the owner of the post
            if (foundPost.UploadedUserId != userFromToken.Id) throw new NoPermissionException("edit");

            // Updates post in database and returns the updated post
            return _mapper.Map<PostDTO>(post);
        }

        /**
         * Deletes post by post id
         * Returns deleted post
         */
        public async Task<PostDTO> DeletePost(Guid postId, string token)
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);

            // Find post in database
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post exists
            if (foundPost == null)
                // Throw an exception if post has not been found
                throw new NotFoundException("Post");

            // Check if user is the owner of the post
            if (foundPost.UploadedByUser.Id != userFromToken.Id) throw new NoPermissionException("delete");

            // Deletes post in database
            var deletedPost = await _postIsqlRepository.Delete(foundPost);

            //Deletes post in graph database
            await _neoRepository.Delete(deletedPost);

            // returns the deleted post
            return _mapper.Map<PostDTO>(deletedPost);
        }

        /**
         * Get all posts from database
         * Returns all posts from database
         */
        public async Task<IEnumerable<PostDTO>> GetPosts()
        {
            // Get all posts from database
            var foundPosts = await _postIsqlRepository.GetAll();

            // Returns found posts
            return _mapper.ProjectTo<PostDTO>(foundPosts as IQueryable);
        }

        /**
         * Add like from a user
         * Returns liked post
         */
        public async Task<PostDTO> UserLikePost(Guid postId, string token)  
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);
            
            // Find post
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");

            // Create UserLike object
            var userLike = new UserLikedPost
            {
                PostId = postId,
                UserId = userFromToken.Id
            };

            // Check if user has already liked this post
            if (userFromToken.LikedPosts.Any(c => c.PostId == postId && c.IsActive))
                // Throw CannotPerformActionException when user has already liked this post
                throw new CannotPerformActionException("You have already liked this post");

            // Add like to post
            foundPost.LikedByUsers.Add(userLike);

            // Update post
            await _postIsqlRepository.Update(foundPost);

            // Insert relation into graph database
            await _neoRepository.CreateRelation(userFromToken, NEORelation.LIKED, foundPost);

            // Return updated post
            return _mapper.Map<PostDTO>(foundPost);
        }

        /**
         * Add dislike from a user
         * Returns disliked post
         */
        public async Task<PostDTO> UserDislikePost(Guid postId, string token)
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);
            
            // Find post
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");

            // Check if user has already disliked this post
            if (userFromToken.DislikedPosts.Any(c => c.PostId == postId && c.IsActive))
                // Throw CannotPerformActionException when user has already disliked this post
                throw new CannotPerformActionException("You have already disliked this post");

            // Create UserDislike object
            var userDislike = new UserDislikedPost
            {
                PostId = postId,
                UserId = userFromToken.Id
            };

            // Add dislike to post
            foundPost.DislikedByUsers.Add(userDislike);

            // Update post
            await _postIsqlRepository.Update(foundPost);

            // Insert relation into graph database
            await _neoRepository.CreateRelation(userFromToken, NEORelation.DISLIKED, foundPost);

            // Return updated post
            return _mapper.Map<PostDTO>(foundPost);
        }

        /**
         * Add view to a post from a user
         * Returns viewed post
         */
        public async Task<PostDTO> UserViewPost(Guid postId, string token)
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);
            
            // Find post
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");

            // Check if user has already viewed this post
            if (userFromToken.ViewedPosts.Any(c => c.PostId == postId && c.IsActive))
                // Return found post if user has already seen this post
                return _mapper.Map<PostDTO>(foundPost);

            // Create UserViewedPost object
            var userView = new UserViewedPost
            {
                PostId = postId,
                UserId = userFromToken.Id
            };

            // Add view to post
            foundPost.ViewedByUsers.Add(userView);

            // Update post
            await _postIsqlRepository.Update(foundPost);

            // Insert relation into graph database
            await _neoRepository.CreateRelation(userFromToken, NEORelation.VIEWED, foundPost);

            // Return updated post
            return _mapper.Map<PostDTO>(foundPost);
        }

        /**
         * Removes dislike on post from a user
         * Returns un- disliked post
         */
        public async Task<PostDTO> UndoUserLikePost(Guid postId, string token)
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);
            
            // Find post
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");

            // Find liked post of user
            var foundLikedPost = userFromToken.LikedPosts.FirstOrDefault(c => c.PostId == postId);

            // Check if like has been found
            if (foundLikedPost == null)
                // Throw NotFoundException if like was not found
                throw new NotFoundException("not disliked");

            // Remove like from post
            await _userLikedIsqlRepository.Delete(foundLikedPost);

            // Removes relation from graph database
            await _neoRepository.RemoveRelation(userFromToken, NEORelation.LIKED, foundPost);

            // Return updated post
            return _mapper.Map<PostDTO>(foundPost);
        }

        /**
         * Removes like on post from a user
         * Returns un- liked post
         */
        public async Task<PostDTO> UndoUserDislikePost(Guid postId, string token)   
        {
            // Get user from token
            var userFromToken = await TokenValidation(token);
            
            // Find post    
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");

            // Find liked post of user
            var foundDislikedPost = userFromToken.DislikedPosts.FirstOrDefault(c => c.PostId == postId);

            // Check if dislike has been found
            if (foundDislikedPost == null)
                // Throw NotFoundException if dislike was not found
                throw new NotFoundException("not disliked");

            // Remove dislike from post
            await _userDislikedIsqlRepository.Delete(foundDislikedPost);

            // Removes relation from graph database
            await _neoRepository.RemoveRelation(userFromToken, NEORelation.DISLIKED, foundPost);

            // Return updated post
            return _mapper.Map<PostDTO>(foundPost);
        }

        /**
         * Get a single post by id
         * Returns a single post
         */
        public async Task<PostDTO> GetPost(Guid postId)
        {
            // Find post in database
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
                // Throw not found exception if post has not been found
                throw new NotFoundException("post");

            // Returns single found post
            return _mapper.Map<PostDTO>(foundPost);
        }

        private Task<Domain.Entities.SQL.User.User> TokenValidation(string token)
        {
            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
                // Throw an exception if token is invalid
                throw new InvalidTokenException();

            // Get user from token
            return _authenticationService.GetUserFromToken(token);
        }
    }
}
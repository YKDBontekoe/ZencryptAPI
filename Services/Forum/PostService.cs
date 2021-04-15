using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Forums;
using Domain.Entities.User;
using Domain.Exceptions;
using Domain.Services.Forum;
using Domain.Services.Repository;
using Domain.Services.User;

namespace Services.Forum
{
    public class PostService : IPostService
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IRepository<Post> _postRepository;

        public PostService(IAuthenticationService authenticationService, IRepository<Post> postRepository)
        {
            _authenticationService = authenticationService;
            _postRepository = postRepository;
        }

        /**
         * Creates a post using an user Id
         * Returns the uploaded post
         */
        public async Task<Post> CreatePost(Post post, string token)
        {
            // Validates Token
            ValidateToken(token);

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);

            // Add user to post
            post.UploadedByUser = tokenUser;

            // Inserts post into database and returns the newly created post
            return  await _postRepository.Insert(post);
        }

        /**
         * Updates a post using post Id
         * Returns updated post
         */
        public async Task<Post> UpdatePost(Guid postId, Post post, string token)
        {
            // Validates Token
            ValidateToken(token);

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);

            // Find post in database
            var foundPost = await _postRepository.Get(postId);

            // Check if post exists
            if (foundPost == null)
            {
                // Throw an exception if post has not been found
                throw new NotFoundException("Post");
            }

            // Set postId in post
            post.Id = postId;

            // Check if user is the owner of the post
            if (foundPost.UploadedUserId != tokenUser.Id)
            {
                throw new NoPermissionException("edit");
            }

            // Updates post in database and returns the updated post
            return await _postRepository.Update(post);
        }

        /**
         * Deletes post by post id
         * Returns deleted post
         */
        public async Task<Post> DeletePost(Guid postId, string token)
        {
            // Validates Token
            ValidateToken(token);

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);

            // Find post in database
            var foundPost = await _postRepository.Get(postId);

            // Check if post exists
            if (foundPost == null)
            {
                // Throw an exception if post has not been found
                throw new NotFoundException("Post");
            }

            // Check if user is the owner of the post
            if (foundPost.UploadedByUser.Id != tokenUser.Id)
            {
                throw new NoPermissionException("delete");
            }

            // Updates post in database and returns the updated post
            return await _postRepository.Delete(foundPost);
        }

        /**
         * Get a single post by id
         * Returns a single post
         */
        public async Task<Post> GetPost(Guid postId)
        {
            // Find post in database
            var foundPost = await _postRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
            {
                // Throw not found exception if post has not been found
                throw new NotFoundException("post");
            }

            // Returns single found post
            return foundPost;
        }

        /**
         * Get all posts from database
         * Returns all posts from database
         */
        public async Task<IEnumerable<Post>> GetPosts()
        {
            // Get all posts from database
            var foundPosts = await _postRepository.GetAll();

            // Check if list is not empty
            if (foundPosts.Any())
            {
                // Throw exception if there aren't any posts
                throw new NotFoundException("posts");
            }

            // Returns found posts
            return foundPosts;
        }

        /**
         * Add like from a user
         * Returns liked post
         */
        public async Task<Post> UserLikePost(Guid postId, string token)
        {
            // Find post
            var foundPost = await _postRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
            {
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");
            }

            // Validates Token
            ValidateToken(token);

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);
            
            // Create UserLike object
            var userLike = new UserLikedPost
            {
                PostId = postId,
                UserId = tokenUser.Id
            };

            // Check if user has already liked this post
            if (tokenUser.LikedPosts.Any(c => c.PostId == postId))
            {
                // Throw CannotPerformActionException when user has already liked this post
                throw new CannotPerformActionException("You have already liked this post");
            }

            // Add like to post
            foundPost.LikedByUsers.Add(userLike);

            // Update post
            await _postRepository.Update(foundPost);

            // Return updated post
            return foundPost;
        }

        /**
         * Add dislike from a user
         * Returns disliked post
         */
        public async Task<Post> UserDislikePost(Guid postId, string token)
        {
            // Find post
            var foundPost = await _postRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
            {
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");
            }

            // Validates Token
            ValidateToken(token);

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);

            // Check if user has already disliked this post
            if (tokenUser.DislikedPosts.Any(c => c.PostId == postId))
            {
                // Throw CannotPerformActionException when user has already disliked this post
                throw new CannotPerformActionException("You have already disliked this post");
            }

            // Create UserDislike object
            var userDislike = new UserDislikedPost
            {
                PostId = postId,
                UserId = tokenUser.Id
            };

            // Add dislike to post
            foundPost.DislikedByUsers.Add(userDislike);

            // Update post
            await _postRepository.Update(foundPost);

            // Return updated post
            return foundPost;
        }

        public async Task<Post> UserViewPost(Guid postId, string token)
        {
            // Find post
            var foundPost = await _postRepository.Get(postId);

            // Check if post is null
            if (foundPost == null)
            {
                // Throw exception if there aren't any posts
                throw new NotFoundException("post");
            }

            // Validates Token
            ValidateToken(token);

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);

            // Check if user has already viewed this post
            if (tokenUser.ViewedPosts.Any(c => c.PostId == postId))
            {
                // Return found post if user has already seen this post
                return foundPost;
            }

            // Create UserViewedPost object
            var userView = new UserViewedPost
            {
                PostId = postId,
                UserId = tokenUser.Id
            };

            // Add view to post
            foundPost.ViewedByUsers.Add(userView);

            // Update post
            await _postRepository.Update(foundPost);

            // Return updated post
            return foundPost;
        }

        private void ValidateToken(string token)
        {
            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                // Throw an exception if token is invalid
                throw new InvalidTokenException();
            }
        }
    }
}

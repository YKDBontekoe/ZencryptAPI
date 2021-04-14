using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums;
using Domain.Entities.Forums;
using Domain.Exceptions;
using Domain.Services;
using Domain.Services.Forum;
using Domain.Services.Repository;
using Domain.Services.User;

namespace Services
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
            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                throw new InvalidTokenException();
            }

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
        public async Task<Post> UpdatePost(Post post, string token)
        {
            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                throw new InvalidTokenException();
            }

            // Get user from token
            var tokenUser = await _authenticationService.GetUserFromToken(token);

            // Find post in database
            var foundPost = await _postRepository.Get(post.Id);

            // Check if post exists
            if (foundPost == null)
            {
                // Throw an exception if post has not been found
                throw new NotFoundException("Post");
            }

            // Check if user is the owner of the post
            if (foundPost.UploadedByUser.Id != tokenUser.Id)
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
            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                throw new InvalidTokenException();
            }

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
    }
}

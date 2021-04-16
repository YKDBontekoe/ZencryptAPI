using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Forums;
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
        private readonly ISQLRepository<Post> _postIsqlRepository;

        public CommentService(IAuthenticationService authenticationService, ISQLRepository<Comment> commentIsqlRepository, ISQLRepository<Post> postIsqlRepository)
        {
            _authenticationService = authenticationService;
            _commentIsqlRepository = commentIsqlRepository;
            _postIsqlRepository = postIsqlRepository;
        }

        /**
         * Create a comment and add the new comment to an existing post
         * Returns the newly created comment
         */
        public async Task<Comment> CreateCommentToPost(Comment comment, Guid postId, string token)
        {
            // Get post from database by Id
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is in database
            if (foundPost == null)
            {
                // Throw error if post is not found/ null
                throw new NotFoundException("Post");
            }

            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                // Throw an exception if token is invalid
                throw new InvalidTokenException();
            }

            // Get user from token
            var userFromToken = await _authenticationService.GetUserFromToken(token);

            // Add userId to comment
            comment.UploadedUserId = userFromToken.Id;

            // Add postId to comment
            comment.PostId = postId;

            // Create and return new comment
            return await _commentIsqlRepository.Insert(comment);
        }

        /**
         * Update a comment and check permission of comment
         * Returns updated comment
         */
        public async Task<Comment> UpdateComment(Guid commentId, Comment comment, string token)
        {
            // Find comment in database
            var foundComment = await _commentIsqlRepository.Get(commentId);

            // Check if comment is in database
            if (foundComment == null)
            {
                // Throw error if comment is not found/ null
                throw new NotFoundException("Comment");
            }

            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                // Throw an exception if token is invalid
                throw new InvalidTokenException();
            }

            // Get user from token
            var userFromToken = await _authenticationService.GetUserFromToken(token);

            // Check if user is owner of comment
            if (foundComment.UploadedUserId != userFromToken.Id)
            {
                // Throw an exception if comment is from user
                throw new NoPermissionException("Comment");
            }

            // Update and return updated comment
            return await _commentIsqlRepository.Update(comment);
        }

        /**
         * Deletes a single comment by id
         * Returns deleted comment
         */
        public async Task<Comment> DeleteComment(Guid commentId, string token)
        {
            // Get comment from database
            var foundComment = await _commentIsqlRepository.Get(commentId);

            // Check if comment is in database
            if (foundComment == null)
            {
                // Throw not found exception if comment is not in database
                throw new NotFoundException("comment");
            }

            // Token validation
            var isValidToken = _authenticationService.IsValidToken(token);

            // Check if token is valid
            if (!isValidToken)
            {
                // Throw an exception if token is invalid
                throw new InvalidTokenException();
            }

            // Get user from token
            var userFromToken = await _authenticationService.GetUserFromToken(token);

            // Check if user is owner of comment
            if (foundComment.UploadedUserId != userFromToken.Id)
            {
                // Throw an exception if comment is from user
                throw new NoPermissionException("Comment");
            }

            // Delete comment and return deleted comment
            return await _commentIsqlRepository.Delete(foundComment);
        }

        /**
         * Returns a single comment by id
         */
        public Task<Comment> GetComment(Guid commentId)
        {
            return _commentIsqlRepository.Get(commentId);
        }

        /**
         * Get all comments from a specific post
         */
        public async Task<IEnumerable<Comment>> GetCommentFromPost(Guid postId)
        {
            // Get post from database by Id
            var foundPost = await _postIsqlRepository.Get(postId);

            // Check if post is in database
            if (foundPost == null)
            {
                // Throw error if post is not found/ null
                throw new NotFoundException("Post");
            }

            // Returns all comments from a post
            return foundPost.Comments;
        }
    }
}

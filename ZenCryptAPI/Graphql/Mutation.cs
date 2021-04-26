using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.User;
using Domain.Entities.SQL.User;
using Domain.Exceptions;
using Domain.Services.Forum;
using Domain.Services.User;
using HotChocolate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ZenCryptAPI.Graphql
{
    public class Mutation
    {
        // ------------------- User ---------------------
        public async Task<TokenUserDTO> GetToken([Service] IAuthenticationService authenticationService,
            LoginUserDTO loginUser)
        {
            var user = await authenticationService.AuthenticateUser(loginUser);
            var token = authenticationService.GetJsonWebToken(user);

            var userDto = new TokenUserDTO
            {
                Token = token,
                UserId = user.Id
            };

            return userDto;
        }

        public async Task<User> CreateUser([Service] IAuthenticationService authenticationService,
            RegisterUserDTO registerUser)
        {
            return await authenticationService.InsertUser(registerUser);
        }

        // ------------------- Post ---------------------
        [Authorize]
        public async Task<PostDTO> CreatePost([Service] IPostService postService, [Service] IHttpContextAccessor contextAccessor, CreatePostDTO createPost)
        {
            return await postService.CreatePost(createPost, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> LikePost([Service] IPostService postService, [Service] IHttpContextAccessor contextAccessor, UserInteractPostDTO interactDto)
        {
            return await postService.UserLikePost(interactDto.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> UndoLikePost([Service] IPostService postService, [Service] IHttpContextAccessor contextAccessor, UserInteractPostDTO interactDto)
        {
            return await postService.UndoUserLikePost(interactDto.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> DislikePost([Service] IPostService postService, [Service] IHttpContextAccessor contextAccessor, UserInteractPostDTO interactDto)
        {
            return await postService.UserDislikePost(interactDto.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> UndoDislikePost([Service] IPostService postService, [Service] IHttpContextAccessor contextAccessor, UserInteractPostDTO interactDto)
        {
            return await postService.UndoUserDislikePost(interactDto.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> ViewPost([Service] IPostService postService, [Service] IHttpContextAccessor contextAccessor, UserInteractPostDTO interactDto)
        {
            return await postService.UserViewPost(interactDto.PostId, GetToken(contextAccessor));
        }

        // ------------------- Comment ---------------------
        [Authorize]
        public async Task<CommentDTO> CreateComment([Service] ICommentService commentService, [Service] IHttpContextAccessor contextAccessor, CreateCommentDTO comment)
        {
            return await commentService.CreateCommentToPost(comment, GetToken(contextAccessor));
        }

        private static string GetToken(IHttpContextAccessor contextAccessor)
        {
            try
            {
                return contextAccessor.HttpContext.Request.Headers.SingleOrDefault(c => c.Key.Equals("Authorization")).Value.ToString().Split(" ")[1];
            }
            catch (Exception e)
            {
                throw new InvalidTokenException();
            }
            
        }
    }
}
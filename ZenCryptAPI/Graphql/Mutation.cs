﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.DataTransferObjects.Forums.Comment.Input;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.Forums.Post.Input;
using Domain.DataTransferObjects.User;
using Domain.DataTransferObjects.User.Input;
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
            LoginUserInput loginUser)
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
            RegisterUserInput registerUser)
        {
            return await authenticationService.InsertUser(registerUser);
        }

        // ------------------- Post ---------------------
        [Authorize]
        public async Task<PostDTO> CreatePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, CreatePostInput createPost)
        {
            return await postService.CreatePost(createPost, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> LikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UserLikePost(interactInput.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> UndoLikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UndoUserLikePost(interactInput.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> DislikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UserDislikePost(interactInput.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> UndoDislikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UndoUserDislikePost(interactInput.PostId, GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> ViewPost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UserViewPost(interactInput.PostId, GetToken(contextAccessor));
        }

        // ------------------- Comment ---------------------
        [Authorize]
        public async Task<CommentDTO> CreateComment([Service] ICommentService commentService,
            [Service] IHttpContextAccessor contextAccessor, CreateCommentInput comment)
        {
            return await commentService.CreateCommentToPost(comment, GetToken(contextAccessor));
        }

        // ------------------- Following ---------------------
        [Authorize]
        public async Task<FollowDTO> FollowUser([Service] IUserService userService,
            [Service] IHttpContextAccessor contextAccessor, CreateFollowInput follow)
        {
            return await userService.FollowUser(GetToken(contextAccessor), follow);
        }

        [Authorize]
        public async Task<UnfollowDTO> UnfollowUser([Service] IUserService userService,
            [Service] IHttpContextAccessor contextAccessor, RemoveFollowInput unfollow)
        {
            return await userService.UnFollowUser(GetToken(contextAccessor), unfollow);
        }

        private static string GetToken(IHttpContextAccessor contextAccessor)
        {
            try
            {
                return contextAccessor.HttpContext.Request.Headers.SingleOrDefault(c => c.Key.Equals("Authorization"))
                    .Value.ToString().Split(" ")[1];
            }
            catch (Exception e)
            {
                throw new InvalidTokenException();
            }
        }
    }
}
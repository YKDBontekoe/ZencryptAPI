using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.User;
using Domain.Entities.SQL.Forums;
using Domain.Entities.SQL.User;
using Domain.Models.User;
using Domain.Services.Forum;
using Domain.Services.User;
using HotChocolate;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.EF.GraphQL
{
    public class Mutation
    {
        // ------------------- User ---------------------
        public async Task<TokenUserDTO> GetToken([Service]IAuthenticationService authenticationService, LoginUserDTO loginUser)
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
        
        public async Task<User> CreateUser([Service]IAuthenticationService authenticationService, RegisterUserDTO registerUser)    
        {
            return await authenticationService.InsertUser(registerUser);
        }
        
        // ------------------- Post ---------------------
        [Authorize]
        public async Task<PostDTO> CreatePost([Service]IPostService postService, CreatePostDTO createPost)
        {
            return await postService.CreatePost(createPost);
        }

        [Authorize]
        public async Task<PostDTO> LikePost([Service] IPostService postService, UserInteractPostDTO interactDto)
        {
            return await postService.UserLikePost(interactDto.PostId, interactDto.UserId);
        }
        
        [Authorize]
        public async Task<PostDTO> UndoLikePost([Service] IPostService postService, UserInteractPostDTO interactDto)
        {
            return await postService.UndoUserLikePost(interactDto.PostId, interactDto.UserId);
        }
        
        [Authorize]
        public async Task<PostDTO> DislikePost([Service] IPostService postService, UserInteractPostDTO interactDto)    
        {
            return await postService.UserDislikePost(interactDto.PostId, interactDto.UserId);
        }
        
        [Authorize] 
        public async Task<PostDTO> UndoDislikePost([Service] IPostService postService, UserInteractPostDTO interactDto)    
        {
            return await postService.UndoUserDislikePost(interactDto.PostId, interactDto.UserId);
        }
        
        [Authorize] 
        public async Task<PostDTO> ViewPost([Service] IPostService postService, UserInteractPostDTO interactDto)    
        {
            return await postService.UserViewPost(interactDto.PostId, interactDto.UserId);
        }
    }
}
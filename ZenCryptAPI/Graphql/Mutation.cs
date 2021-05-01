using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.DataTransferObjects.Forums.Comment.Input;
using Domain.DataTransferObjects.Forums.Forum;
using Domain.DataTransferObjects.Forums.Forum.Input;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.Forums.Post.Input;
using Domain.DataTransferObjects.User;
using Domain.DataTransferObjects.User.Input;
using Domain.Entities.SQL.User;
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

        // ------------------- Forum ---------------------
        [Authorize]
        public async Task<ForumDTO> CreateForum([Service] IForumService forumService,
            [Service] IHttpContextAccessor contextAccessor, CreateForumInput createForum)
        {
            return await forumService.CreateForum(createForum, TokenHandler.GetToken(contextAccessor));
        }
        
        [Authorize]
        public async Task<ForumDTO> FollowForum([Service] IForumService forumService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractForumInput interactForum)
        {
            return await forumService.FollowForum(interactForum.ForumId, TokenHandler.GetToken(contextAccessor));
        }
        
        [Authorize]
        public async Task<ForumDTO> UnFollowForum([Service] IForumService forumService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractForumInput interactForum)
        {
            return await forumService.UnfollowForum(interactForum.ForumId, TokenHandler.GetToken(contextAccessor));
        }
        
        [Authorize]
        public async Task<ForumDTO> HideForum([Service] IForumService forumService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractForumInput interactForum)
        {
            return await forumService.HideForum(interactForum.ForumId, TokenHandler.GetToken(contextAccessor));
        }
        
        [Authorize]
        public async Task<ForumDTO> UnHideForum([Service] IForumService forumService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractForumInput interactForum)
        {
            return await forumService.UnHideForum(interactForum.ForumId, TokenHandler.GetToken(contextAccessor));
        }
        // ------------------- Post ---------------------
        [Authorize]
        public async Task<PostDTO> CreatePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, CreatePostInput createPost)
        {
            return await postService.CreatePost(createPost, TokenHandler.GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> LikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UserLikePost(interactInput.PostId, TokenHandler.GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> UndoLikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UndoUserLikePost(interactInput.PostId, TokenHandler.GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> DislikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UserDislikePost(interactInput.PostId, TokenHandler.GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> UndoDislikePost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UndoUserDislikePost(interactInput.PostId, TokenHandler.GetToken(contextAccessor));
        }

        [Authorize]
        public async Task<PostDTO> ViewPost([Service] IPostService postService,
            [Service] IHttpContextAccessor contextAccessor, UserInteractPostInput interactInput)
        {
            return await postService.UserViewPost(interactInput.PostId, TokenHandler.GetToken(contextAccessor));
        }

        // ------------------- Comment ---------------------
        [Authorize]
        public async Task<CommentDTO> CreateComment([Service] ICommentService commentService,
            [Service] IHttpContextAccessor contextAccessor, CreateCommentInput comment)
        {
            return await commentService.CreateCommentToPost(comment, TokenHandler.GetToken(contextAccessor));
        }

        // ------------------- Following ---------------------
        [Authorize]
        public async Task<FollowDTO> FollowUser([Service] IUserService userService,
            [Service] IHttpContextAccessor contextAccessor, CreateFollowInput follow)
        {
            return await userService.FollowUser(TokenHandler.GetToken(contextAccessor), follow);
        }

        [Authorize]
        public async Task<UnfollowDTO> UnfollowUser([Service] IUserService userService,
            [Service] IHttpContextAccessor contextAccessor, RemoveFollowInput unfollow)
        {
            return await userService.UnFollowUser(TokenHandler.GetToken(contextAccessor), unfollow);
        }
    }
}
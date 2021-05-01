using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.Forums.Post.Input;
using Domain.Entities.SQL.Forums;

namespace Domain.Services.Forum
{
    public interface IPostService
    {
        Task<PostDTO> CreatePost(CreatePostInput createPost, string token);
        Task<PostDTO> UpdatePost(Guid postId, Post post, string token);
        Task<PostDTO> DeletePost(Guid postId, string token);
        Task<IEnumerable<PostDTO>> GetPosts(Guid? forumId);

        Task<PostDTO> UserLikePost(Guid postId, string token);
        Task<PostDTO> UserDislikePost(Guid postId, string token);
        Task<PostDTO> UserViewPost(Guid postId, string token);

        Task<PostDTO> UndoUserLikePost(Guid postId, string token);
        Task<PostDTO> UndoUserDislikePost(Guid postId, string token);
    }
}
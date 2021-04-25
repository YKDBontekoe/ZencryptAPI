using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Post;
using Domain.Entities.SQL.Forums;
using Domain.Enums;

namespace Domain.Services.Forum
{
    public interface IPostService
    {
        Task<PostDTO> CreatePost(CreatePostDTO createPost);
        Task<PostDTO> UpdatePost(Guid postId, Post post, string token);
        Task<PostDTO> DeletePost(Guid postId, string token);
        Task<IEnumerable<PostDTO>> GetPosts();

        Task<PostDTO> UserLikePost(Guid postId, Guid userId);
        Task<PostDTO> UserDislikePost(Guid postId, Guid userId);
        Task<PostDTO> UserViewPost(Guid postId, Guid userId);   

        Task<PostDTO> UndoUserLikePost(Guid postId, Guid userId);
        Task<PostDTO> UndoUserDislikePost(Guid postId, Guid userId);
    }
}
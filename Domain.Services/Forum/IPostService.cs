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
        Task<Post> CreatePost(CreatePostDTO createPost, string token);
        Task<Post> UpdatePost(Guid postId, Post post, string token);
        Task<Post> DeletePost(Guid postId, string token);
        Task<Post> GetPost(Guid postId);
        Task<IEnumerable<Post>> GetPosts(ApiSortType? sortType, string? searchWord, int? pageSize, int? page);

        Task<Post> UserLikePost(Guid postId, string token);
        Task<Post> UserDislikePost(Guid postId, string token);
        Task<Post> UserViewPost(Guid postId, string token);

        Task<Post> UndoUserLikePost(Guid postId, string token);
        Task<Post> UndoUserDislikePost(Guid postId, string token);
    }
}
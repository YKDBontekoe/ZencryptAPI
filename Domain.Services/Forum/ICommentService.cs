using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.SQL.Forums;

namespace Domain.Services.Forum
{
    public interface ICommentService
    {
        Task<Comment> CreateCommentToPost(Comment comment, Guid postId, string token);
        Task<Comment> UpdateComment(Guid commentId, Comment comment, string token);
        Task<Comment> DeleteComment(Guid commentId, string token);
        Task<Comment> GetComment(Guid commentId);
        Task<IEnumerable<Comment>> GetCommentFromPost(Guid postId);
    }
}
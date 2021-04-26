using System;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Comment;
using Domain.Entities.SQL.Forums;

namespace Domain.Services.Forum
{
    public interface ICommentService
    {
        Task<CommentDTO> CreateCommentToPost(CreateCommentDTO comment);
        Task<CommentDTO> UpdateComment(Guid commentId, Comment comment, string token);
        Task<CommentDTO> DeleteComment(Guid commentId, string token);
        Task<CommentDTO> GetComment(Guid commentId);
    }
}
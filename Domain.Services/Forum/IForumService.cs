using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DataTransferObjects.Forums.Forum;
using Domain.DataTransferObjects.Forums.Forum.Input;

namespace Domain.Services.Forum
{
    public interface IForumService
    {
        Task<ForumDTO> CreateForum(CreateForumInput createForum, string token);
        Task<ForumDTO> UpdateForum(Guid forumId, Entities.SQL.Forums.Forum post, string token);
        Task<ForumDTO> DeleteForum(Guid postId, string token);
        Task<IEnumerable<ForumDTO>> GetForums();
    }
}
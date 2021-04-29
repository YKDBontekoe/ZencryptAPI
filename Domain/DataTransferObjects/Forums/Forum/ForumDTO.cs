using System;
using System.Collections.Generic;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.User;

namespace Domain.DataTransferObjects.Forums.Forum
{
    public class ForumDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public SafeUserDTO CreatedByUser { get; set; }
        public ICollection<PostDTO> Posts { get; set; }
    }
}
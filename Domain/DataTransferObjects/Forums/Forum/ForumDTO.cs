using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DataTransferObjects.Forums.Post;
using Domain.DataTransferObjects.User;

namespace Domain.DataTransferObjects.Forums.Forum
{
    public class ForumDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string  Description { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
        public int TotalViews { get; set; }
        public int  TotalFollowers { get; set; }


        public SafeUserDTO CreatedByUser { get; set; }
        public ICollection<PostDTO> Posts { get; set; }
    }
}
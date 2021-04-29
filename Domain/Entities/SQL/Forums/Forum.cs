using System;
using System.Collections.Generic;
using Domain.Entities.SQL.User.Links;

namespace Domain.Entities.SQL.Forums
{
    public class Forum : BaseEntity
    {
        public string Title { get; set; }
        public Guid CreatedByUserId { get; set; }
        public Guid ForumTypeId { get; set; }


        public virtual ICollection<Post> Posts { get; set; }
        public virtual ForumType ForumType { get; set; }
        public virtual User.User CreatedByUser { get; set; }
        public virtual ICollection<UserFollowingForum> FollowedByUsers { get; set; }
        public virtual ICollection<UserHiddenForum> HiddenByUsers { get; set; }
    }
}
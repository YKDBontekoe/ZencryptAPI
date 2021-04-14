using System;
using System.Collections.Generic;
using Domain.Entities.User;

namespace Domain.Entities.Forums
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public Guid UploadedUserId { get; set; }

        public virtual ICollection<UserLikedPost> LikedByUsers { get; set; }
        public virtual ICollection<UserDislikedPost> DislikedByUsers { get; set; }
        public virtual ICollection<UserViewedPost> ViewedByUsers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual User.User UploadedByUser { get; set; }
    }
}

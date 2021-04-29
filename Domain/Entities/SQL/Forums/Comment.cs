using System;
using System.Collections.Generic;
using Domain.Entities.SQL.User;
using Domain.Entities.SQL.User.Links;

namespace Domain.Entities.SQL.Forums
{
    public class Comment : BaseEntity
    {
        public string Description { get; set; }
        public Guid UploadedUserId { get; set; }
        public Guid PostId { get; set; }

        public virtual User.User UploadedUser { get; set; }
        public virtual Post OriginPost { get; set; }
        public virtual ICollection<UserLikedComment> LikedByUsers { get; set; }
        public virtual ICollection<UserDislikedComment> DislikedByUsers { get; set; }
    }
}
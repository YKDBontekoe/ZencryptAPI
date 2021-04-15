using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.User;

namespace Domain.Entities.Forums
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

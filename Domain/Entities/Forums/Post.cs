using System.Collections.Generic;

namespace Domain.Entities.Forums
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<User.User> LikedByUsers { get; set; }  
        public virtual User.User UploadedByUser { get; set; }
    }
}

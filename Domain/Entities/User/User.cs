using System.Collections.Generic;
using Domain.Entities.Forums;

namespace Domain.Entities.User
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Post> UploadedPosts { get; set; }
        public virtual ICollection<Comment> UploadedComments { get; set; }

        public virtual ICollection<Post> LikedPosts { get; set; }
        public virtual ICollection<Comment> LikedComments { get; set; }
    }
}

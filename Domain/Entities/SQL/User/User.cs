using System.Collections.Generic;
using Domain.Entities.SQL.Forums;

namespace Domain.Entities.SQL.User
{
    public class User : BaseEntity
    {
        public string? UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }


        public virtual ICollection<Post> UploadedPosts { get; set; }
        public virtual ICollection<Comment> UploadedComments { get; set; }

        public virtual ICollection<UserViewedPost> ViewedPosts { get; set; }

        public virtual ICollection<UserLikedPost> LikedPosts { get; set; }
        public virtual ICollection<UserLikedComment> LikedComments { get; set; }

        public virtual ICollection<UserDislikedPost> DislikedPosts { get; set; }
        public virtual ICollection<UserDislikedComment> DislikedComments { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Forums
{
    public class Comment : BaseEntity
    {
        public string Description { get; set; }

        public virtual User.User UploadedUser { get; set; }
        public virtual ICollection<User.User> LikedByUsers { get; set; }
    }
}

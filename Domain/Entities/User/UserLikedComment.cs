using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Forums;

namespace Domain.Entities.User
{
    public class UserLikedComment : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid CommentId { get; set; }

        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}

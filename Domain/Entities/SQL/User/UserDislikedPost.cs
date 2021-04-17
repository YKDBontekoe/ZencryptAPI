using System;
using Domain.Entities.SQL.Forums;

namespace Domain.Entities.SQL.User
{
    public class UserDislikedPost : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        public virtual User User { get; set; }
        public virtual Post Post { get; set; } 
    }
}

using System;
using Domain.Entities.SQL.Forums;

namespace Domain.Entities.SQL.User.Links
{
    public class UserHiddenForum : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }

        public virtual User User { get; set; }
        public virtual Forum Forum { get; set; }
    }
}
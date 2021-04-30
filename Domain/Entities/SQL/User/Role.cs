using System.Collections.Generic;
using Domain.Entities.SQL.User.Links;

namespace Domain.Entities.SQL.User
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
using System.Collections.Generic;

namespace Domain.Entities.SQL.Forums
{
    public class ForumType : BaseEntity
    {
        public string TypeName { get; set; }

        public virtual ICollection<Forum> Forums { get; set; }
    }
}
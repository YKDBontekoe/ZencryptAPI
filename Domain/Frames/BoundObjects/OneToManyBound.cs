using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Frames.BoundObjects
{
    public class OneToManyBound<TA, TB> where TA : BaseEntity where TB : BaseEntity
    {
        public TA ObjectA { get; set; }
        public NEO RelationShip { get; set; }
        public IEnumerable<TB> ObjectList { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Neo;
using Domain.Enums;
using Domain.Enums.Neo;

namespace Domain.Frames.BoundObjects
{
    public class OneToManyBound<TA, TB> where TA : BaseEntity where TB : BaseEntity
    {
        public TA ObjectA { get; set; }
        public NEORelation RelationShip { get; set; }
        public IEnumerable<TB> ObjectList { get; set; } 
    }

}

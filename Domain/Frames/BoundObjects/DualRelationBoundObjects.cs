using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Neo;

namespace Domain.Frames.BoundObjects
{
    public class DualRelationBoundObjects<TA, TB> where TA : BaseEntity where TB : BaseEntity
    {
        public TA ObjectA { get; set; }
        public NEORelation RelationShip { get; set; }
        public TB ObjectC { get; set; }
    }
}

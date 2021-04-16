using Domain.Entities;
using Domain.Enums;

namespace Domain.Frames.BoundObjects
{
    public class DualNeoRelationBoundObjects<TA, TB> where TA : BaseEntity where TB : BaseEntity
    {
        public TA ObjectA { get; set; }
        public NEO RelationShip { get; set; }
        public TB ObjectC { get; set; }
    }
}

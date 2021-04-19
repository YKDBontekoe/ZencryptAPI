using Domain.Entities.Neo;
using Domain.Enums;
using Domain.Enums.Neo;

namespace Domain.Frames.BoundObjects.Neo
{
    public class DualNeoRelationBoundObjects
    {
        public NeoEntity ObjectA { get; set; }
        public NEORelation RelationShip { get; set; }
        public NeoEntity ObjectC { get; set; }
    }
}

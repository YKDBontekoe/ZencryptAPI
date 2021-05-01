using System.Collections.Generic;
using Domain.Entities.Neo;
using Domain.Enums.Neo;

namespace Domain.Frames.BoundObjects.Neo
{
    public class OneToManyBoundNeo
    {
        public NeoEntity ObjectA { get; set; }
        public NEORelation RelationShip { get; set; }
        public IEnumerable<NeoEntity> ObjectList { get; set; }
    }
}
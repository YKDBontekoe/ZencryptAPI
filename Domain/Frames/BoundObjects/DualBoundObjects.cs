using Domain.Entities;

namespace Domain.Frames.BoundObjects
{
    public class DualBoundObjects<TA, TB> where TA : BaseEntity where TB : BaseEntity
    {
        public TA ObjectA { get; set; } 
        public TB ObjectB { get; set; }
    }
}

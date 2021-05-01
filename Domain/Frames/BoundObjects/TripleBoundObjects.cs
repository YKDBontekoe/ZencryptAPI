using Domain.Entities;

namespace Domain.Frames.BoundObjects
{
    public class TripleBoundObjects<TA, TB, TC> where TA : BaseEntity where TB : BaseEntity where TC : BaseEntity
    {
        public TA ObjectA { get; set; }
        public TB ObjectB { get; set; }
        public TC ObjectC { get; set; }
    }
}
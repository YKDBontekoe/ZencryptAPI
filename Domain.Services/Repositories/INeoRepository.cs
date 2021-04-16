using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Frames.BoundObjects;

namespace Domain.Services.Repositories
{
    public interface INeoRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<DualNeoRelationBoundObjects<TA, TB>>> GetNodesWithRelation<TA, TB>(TA entityA, NEO relationType, TB entityB) 
            where TA : BaseEntity where TB : BaseEntity;  
        Task<IEnumerable<DualBoundObjects<TA, TB>>> GetNodesWithoutRelation<TA, TB>(TA entityA, TB entityB) 
            where TA : BaseEntity where TB : BaseEntity;
        Task<DualNeoRelationBoundObjects<TA, TB>> CreateRelation<TA, TB>(TA entityA, NEO relation, TB entityB)
            where TA : BaseEntity where TB : BaseEntity;
    }
}
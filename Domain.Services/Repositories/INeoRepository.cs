using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Enums.Neo;
using Domain.Frames.BoundObjects;
using Domain.Frames.BoundObjects.Neo;

namespace Domain.Services.Repositories
{
    public interface INeoRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<DualRelationBoundObjects<TA, TB>>> GetNodesWithRelation<TA, TB>(TA entityA, NEORelation relationType, TB entityB) 
            where TA : BaseEntity where TB : BaseEntity;

        Task<OneToManyBoundNeo> GetNodeWithRelatedNodes<TA, TB>(Guid entityId, NEORelation relation, NeoRelationType neoRelationType)
            where TA : BaseEntity where TB : BaseEntity;
        Task<IEnumerable<DualBoundObjects<TA, TB>>> GetNodesWithoutRelation<TA, TB>(TA entityA, TB entityB) 
            where TA : BaseEntity where TB : BaseEntity;
        Task<DualRelationBoundObjects<TA, TB>> CreateRelation<TA, TB>(TA entityA, NEORelation relation, TB entityB)
            where TA : BaseEntity where TB : BaseEntity;
        Task<DualRelationBoundObjects<TA, TB>> RemoveRelation<TA, TB>(TA entityA, NEORelation relation, TB entityB)
            where TA : BaseEntity where TB : BaseEntity;
    }
}
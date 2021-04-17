using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Neo;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Frames.BoundObjects;
using Domain.Services.Repositories;
using Microsoft.Extensions.Configuration;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Infrastructure.EF.Repositories
{
    public class NeoRepository<T> : INeoRepository<T> where T : BaseEntity
    {
        private readonly IGraphClient _client;
        private readonly string _objName = typeof(T).Name;
        private readonly ISQLRepository<T> _sqlRepository;

        public NeoRepository(IGraphClient client, ISQLRepository<T> sqlRepository)
        {
            _client = client;
            _sqlRepository = sqlRepository;
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return _client.Cypher
                .Match($"(e:{_objName})")
                .Return(e => e.As<T>())
                .ResultsAsync;
        }

        public async Task<T> Get(Guid id)
        {
                var item =  await _client.Cypher
                .Match($"(item:{_objName})")
                .Where((T item) => item.Id == id)
                .Return(user => user.As<NeoEntity>())
                .ResultsAsync;

                var foundItem = item.FirstOrDefault();
                
                // Check if found item is not null
                if (foundItem == null)
                {
                    throw new NotFoundException(_objName + " in neo");
                }

                var temp = Guid.Parse(foundItem.EntityId);

                return await _sqlRepository.Get(temp);
        }

        public async Task<T> Insert(T entity)
        {
            await _client.Cypher
                .Merge("(item:" + _objName + "{ EntityId : \"" + entity.Id + "\", entityType: \"" + _objName  + "\" })")
                .ExecuteWithoutResultsAsync();

            return entity;
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public async Task<T> Delete(T entity)
        {
            await _client.Cypher
                .OptionalMatch($"(item:{_objName})<-[r]-()")
                .Where((T item) => item.Id == entity.Id)
                .Delete("r, item")
                .ExecuteWithoutResultsAsync();

            return entity;
        }

        public Task<IEnumerable<DualNeoRelationBoundObjects<TA, TB>>> GetNodesWithRelation<TA, TB>(TA entityA, NEO relationType, TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DualBoundObjects<TA, TB>>> GetNodesWithoutRelation<TA, TB>(TA entityA, TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<DualNeoRelationBoundObjects<TA, TB>> CreateRelation<TA, TB>(TA entityA, NEO relation, TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
             await _client.Cypher
                .Match("(item1:" + typeof(TA).Name + ")", "(item2:" + typeof(TB).Name + ") WHERE (item1.EntityId = '" + entityA.Id + "') AND (item2.EntityId = '" + entityB.Id + "')")
                .Create($"(item1)-[r:{relation}]->(item2)")
                .ExecuteWithoutResultsAsync();

           var itemRelationObject = new DualNeoRelationBoundObjects<TA, TB>
           {
               ObjectA = entityA,
               RelationShip = relation,
               ObjectC = entityB
           };

           return itemRelationObject;
        }

        public async Task<DualNeoRelationBoundObjects<TA, TB>> RemoveRelation<TA, TB>(TA entityA, NEO relation, TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
            await _client.Cypher
                .Match($"(item1:{typeof(TA).Name})-[r]->({typeof(TB).Name}) WHERE (item1.EntityId = '" + entityA.Id + "') AND (item2.EntityId = '" + entityB.Id + "') AND type(r) = ('" + relation + "')")
                .Delete("r").ExecuteWithoutResultsAsync();

            var itemRelationObject = new DualNeoRelationBoundObjects<TA, TB> 
            {
                ObjectA = entityA,
                RelationShip = relation,
                ObjectC = entityB
            };

            return itemRelationObject;
        }
    }
}

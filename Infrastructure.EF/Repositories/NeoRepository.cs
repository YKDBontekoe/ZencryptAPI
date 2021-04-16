using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Frames.BoundObjects;
using Domain.Services.Repositories;
using Microsoft.Extensions.Configuration;
using Neo4jClient;

namespace Infrastructure.EF.Repositories
{
    public class NeoRepository<T> : INeoRepository<T> where T : BaseEntity
    {
        private readonly IGraphClient _client;

        public NeoRepository(NeoServerConfiguration _configuration)
        {
            _client.ConnectAsync(_configuration);
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return _client.Cypher
                .Match($"(e:{typeof(T)})")
                .Return(e => e.As<T>())
                .ResultsAsync;
        }

        public async Task<T> Get(Guid id)
        {
                var item =  await _client.Cypher
                .Match($"(item:{typeof(T)})")
                .Where((T item) => item.Id == id)
                .Return(user => user.As<T>())
                .ResultsAsync;

                return item.FirstOrDefault();
        }

        public async Task<T> Insert(T entity)
        {
            await _client.Cypher
                .Merge("(item:" +  typeof(T) + "{ id: {" + entity.Id + "} })")
                .OnCreate()
                .Set("item = {new" + typeof(T) + "}")
                .WithParams(new
                {
                    id = entity.Id,
                    entityType = typeof(T)
                })
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
                .OptionalMatch($"(item:{typeof(T)})<-[r]-()")
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
                .Match("(user1:User)", "(user2:User)")
                .Where((TA item1) => item1.Id == entityA.Id)
                .AndWhere((TB item2) => item2.Id == entityB.Id)
                .CreateUnique($"item1-[:{relation}]->item2")
                .ExecuteWithoutResultsAsync();

           var itemRelationObject = new DualNeoRelationBoundObjects<TA, TB>()
           {
               ObjectA = entityA,
               RelationShip = relation,
               ObjectC = entityB
           };

           return itemRelationObject;
        }
    }
}

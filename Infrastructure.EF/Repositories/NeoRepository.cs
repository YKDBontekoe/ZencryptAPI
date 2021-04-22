using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Entities.Neo;
using Domain.Enums.Neo;
using Domain.Exceptions;
using Domain.Frames.BoundObjects;
using Domain.Frames.BoundObjects.Neo;
using Domain.Services.Repositories;
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
            var item = await _client.Cypher
                .Match($"(item:{_objName})")
                .Where((T item) => item.Id == id)
                .Return(user => user.As<NeoEntity>())
                .ResultsAsync;

            var foundItem = item.FirstOrDefault();

            // Check if found item is not null
            if (foundItem == null) throw new NotFoundException(_objName + " in neo");

            var temp = Guid.Parse(foundItem.EntityId);

            return await _sqlRepository.Get(temp);
        }

        public async Task<T> Insert(T entity)
        {
            await _client.Cypher
                .Merge("(item:" + _objName + "{ EntityId : \"" + entity.Id + "\", entityType: \"" + _objName + "\" })")
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

        public Task<IEnumerable<DualRelationBoundObjects<TA, TB>>> GetNodesWithRelation<TA, TB>(TA entityA,
            NEORelation relationType, TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<OneToManyBoundNeo> GetNodeWithRelatedNodes<TA, TB>(Guid entityId, NEORelation relation,
            NeoRelationType neoRelationType) where TA : BaseEntity where TB : BaseEntity
        {
            var result = await
                QueryBuilder(neoRelationType, relation, false, typeof(TA).Name, typeof(TB).Name,
                        entityId.ToString().ToLower(), null)
                    .Return((item1, item2) =>
                        new
                        {
                            entityA = item1.As<NeoEntity>(),
                            entityB = item2.As<NeoEntity>()
                        }
                    )
                    .ResultsAsync;

            if (result.Any())
            {
                var oneToManyResult = new OneToManyBoundNeo
                {
                    ObjectA = result.FirstOrDefault().entityA,
                    ObjectList = result.Select(c => c.entityB),
                    RelationShip = relation
                };
                return oneToManyResult;
            }

            return null;
        }

        public Task<IEnumerable<DualBoundObjects<TA, TB>>> GetNodesWithoutRelation<TA, TB>(TA entityA, TB entityB)
            where TA : BaseEntity where TB : BaseEntity
        {
            throw new NotImplementedException();
        }

        public async Task<DualRelationBoundObjects<TA, TB>> CreateRelation<TA, TB>(TA entityA, NEORelation relation,
            TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
            await _client.Cypher
                .Match("(item1:" + typeof(TA).Name + ")",
                    "(item2:" + typeof(TB).Name + ") WHERE (item1.EntityId = '" + entityA.Id +
                    "') AND (item2.EntityId = '" + entityB.Id + "')")
                .Create($"(item1)-[r:{relation}]->(item2)")
                .ExecuteWithoutResultsAsync();

            var itemRelationObject = new DualRelationBoundObjects<TA, TB>
            {
                ObjectA = entityA,
                RelationShip = relation,
                ObjectC = entityB
            };

            return itemRelationObject;
        }

        public async Task<DualRelationBoundObjects<TA, TB>> RemoveRelation<TA, TB>(TA entityA, NEORelation relation,
            TB entityB) where TA : BaseEntity where TB : BaseEntity
        {
            await _client.Cypher
                .Match($"(item1:{typeof(TA).Name})-[r]->(item2:{typeof(TB).Name}) WHERE (item1.EntityId = '" +
                       entityA.Id + "') AND (item2.EntityId = '" + entityB.Id + "') AND type(r) = ('" + relation + "')")
                .Delete("r").ExecuteWithoutResultsAsync();

            var itemRelationObject = new DualRelationBoundObjects<TA, TB>
            {
                ObjectA = entityA,
                RelationShip = relation,
                ObjectC = entityB
            };

            return itemRelationObject;
        }

        private ICypherFluentQuery QueryBuilder(NeoRelationType relationType, NEORelation relation, bool isOptional,
            string entityAName, string entityBName, string entityAId, string? entityBId)
        {
            switch (relationType)
            {
                case NeoRelationType.RIGHT:
                {
                    var query =
                        $"(item1:{entityAName})-[rel:{relation}]->(item2:{entityBName}) WHERE (item1.EntityId = '{entityAId}')";

                    return isOptional ? _client.Cypher.OptionalMatch(query) : _client.Cypher.Match(query);
                }

                case NeoRelationType.INNER:
                {
                    var query =
                        $"(item1:{entityAName})->[rel:]<-(item2:{entityBName}) WHERE (item1.EntityId = '{entityAId}' AND item2.EntityId= '{entityBId}')";

                    return isOptional ? _client.Cypher.OptionalMatch(query) : _client.Cypher.Match(query);
                }

                case NeoRelationType.OUTER:
                {
                    var query =
                        $"(item1:{entityAName})<-[rel:{relation}]->(item2:{entityBName}) WHERE (item1.EntityId = '{entityAId}' AND item2.EntityId= '{entityBId}')";

                    return isOptional ? _client.Cypher.OptionalMatch(query) : _client.Cypher.Match(query);
                }

                case NeoRelationType.SHORTEST_PATH:
                {
                    var query = "(item1: " + entityAName + " {EntityId:'" + entityAId + "'}), (item2:" + entityBName +
                                " {EntityId:'" + entityBId +
                                "'), p = shortestPath((item1)-[*]-(item2)) WHERE length(p) > 1";

                    return isOptional ? _client.Cypher.OptionalMatch(query) : _client.Cypher.Match(query);
                }

                case NeoRelationType.LEFT:
                {
                    var query =
                        $"(item1:{entityAName})<-[rel:{relation}]-(item2:{entityBName}) WHERE (item1.EntityId = '{entityAId}')";

                    return isOptional ? _client.Cypher.OptionalMatch(query) : _client.Cypher.Match(query);
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(relationType), relationType, null);
            }
        }
    }
}
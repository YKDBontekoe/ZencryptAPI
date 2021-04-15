using Domain.Entities;
using Domain.Services;
using Infrastructure.EF.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Services.Repository;


namespace Infrastructure.EF
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbSet<T> _entities;
        private readonly EntityContext _entityContext;

        public SQLRepository(EntityContext entityContext)
        {
            _entityContext = entityContext;
            _entities = entityContext.Set<T>();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult(_entities.Where(e => e.IsActive).AsEnumerable());
        }

        public Task<T> Get(Guid id)
        {
            return _entities.SingleOrDefaultAsync(s => s.Id == id && s.IsActive);
        }

        public async Task<T> Insert(T entity)
        {
            IsNull(entity);

            _entities.Add(entity);
            await _entityContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Update(T entity)
        {
            IsNull(entity);

            _entityContext.Entry(entity).State = EntityState.Modified;
            _entities.Update(entity);
            await _entityContext.SaveChangesAsync();

            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            IsNull(entity);

            entity.IsActive = false;

            _entities.Update(entity);
            await _entityContext.SaveChangesAsync();

            return entity;
        }

        public Task<IEnumerable<T>> Filter(Func<T, bool> filterExpression)
        {
            return Task.FromResult(_entities.Where(filterExpression).AsEnumerable());
        }

        public Task SaveChanges()
        {
            return _entityContext.SaveChangesAsync();
        }

        private static void IsNull(T value)
        {
            if (value == null) throw new ArgumentException("entity is null");
        }
    }
}

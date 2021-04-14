using Domain.Entities;
using Domain.Entities.User;
using Infrastructure.EF.EntityMapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF.Context
{
    public class EntityContext : DbContext
    {
        public EntityContext(DbContextOptions<EntityContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            UserMap.CreateMap(modelBuilder.Entity<User>());
        }
    }
}

using Domain.Entities.User;
using Infrastructure.EF.EntityMapping;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Domain.Entities.Forums;

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
            PostMap.CreateMap(modelBuilder.Entity<Post>());
            CommentMap.CreateMap(modelBuilder.Entity<Comment>());
        }
    }
}

using Domain.Entities.SQL.Forums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityMapping
{
    public static class ForumMap
    {
        public static void CreateMap(EntityTypeBuilder<Forum> entityBuilder)
        {
            entityBuilder.HasKey(p => p.Id);

            entityBuilder.HasOne(p => p.ForumType)
                .WithMany(c => c.Forums)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
using Domain.Entities.SQL.Forums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityMapping
{
    public static class CommentMap
    {
        public static void CreateMap(EntityTypeBuilder<Comment> entityBuilder)
        {
            entityBuilder.HasKey(c => c.Id);

            entityBuilder.HasMany(c => c.LikedByUsers)
                .WithOne(c => c.Comment)
                .HasForeignKey(f => f.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(c => c.DislikedByUsers)
                .WithOne(c => c.Comment)
                .HasForeignKey(f => f.CommentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
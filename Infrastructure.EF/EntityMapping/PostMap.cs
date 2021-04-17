using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.SQL.Forums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityMapping
{
    public static class PostMap
    {
        public static void CreateMap(EntityTypeBuilder<Post> entityBuilder)
        {
            entityBuilder.HasKey(p => p.Id);

            entityBuilder.HasMany(p => p.Comments)
                .WithOne(c => c.OriginPost)
                .HasForeignKey(f => f.PostId);

            entityBuilder.HasMany(p => p.DislikedByUsers)
                .WithOne(p => p.Post)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(p => p.LikedByUsers)
                .WithOne(p => p.Post)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(p => p.ViewedByUsers)
                .WithOne(p => p.Post)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

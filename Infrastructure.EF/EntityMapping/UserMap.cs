using Domain.Entities;
using Domain.Entities.User;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityMapping
{
    public static class UserMap
    {
        public static void CreateMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(u => u.Id);
            entityBuilder.HasIndex(u => u.Email);

            entityBuilder.HasMany(u => u.UploadedPosts).WithOne(p => p.UploadedByUser);
            entityBuilder.HasMany(u => u.UploadedComments).WithOne(c => c.UploadedUser);

            entityBuilder.HasMany(u => u.LikedPosts).WithMany(p => p.LikedByUsers);
            entityBuilder.HasMany(u => u.LikedComments).WithMany(c => c.LikedByUsers);
        }

    }
}
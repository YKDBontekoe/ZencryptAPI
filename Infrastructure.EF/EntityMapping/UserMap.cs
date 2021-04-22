using Domain.Entities.SQL.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityMapping
{
    public static class UserMap
    {
        public static void CreateMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(u => u.Id);
            entityBuilder.HasIndex(u => u.Email);

            entityBuilder.HasMany(u => u.UploadedPosts)
                .WithOne(p => p.UploadedByUser)
                .HasForeignKey(f => f.UploadedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(u => u.UploadedComments)
                .WithOne(c => c.UploadedUser)
                .HasForeignKey(f => f.UploadedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(u => u.LikedPosts)
                .WithOne(p => p.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(u => u.DislikedPosts)
                .WithOne(p => p.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(u => u.LikedComments)
                .WithOne(c => c.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(u => u.DislikedComments)
                .WithOne(c => c.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder.HasMany(u => u.ViewedPosts)
                .WithOne(c => c.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EF.EntityMapping
{
    public static class UserMap
    {
        public static void CreateMap(EntityTypeBuilder<User> entityBuilder)
        {
            entityBuilder.HasKey(u => u.Id);
            entityBuilder.HasIndex(u => u.Email);
        }

    }
}
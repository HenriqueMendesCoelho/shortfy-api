using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using suavesabor_api.User.Domain;

namespace suavesabor_api.Application.Data.TypeConfiguration
{
    public class UserRoleTypeConfiguration : IEntityTypeConfiguration<UserRoleDomain>
    {
        public void Configure(EntityTypeBuilder<UserRoleDomain> builder)
        {
            builder.HasIndex(ur => new { ur.UserId, ur.Role }).IsUnique(true);
        }
    }
}

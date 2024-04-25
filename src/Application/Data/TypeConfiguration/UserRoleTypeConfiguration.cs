using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shortfy_api.User.Domain;

namespace shortfy_api.src.Application.Data.TypeConfiguration
{
    public class UserRoleTypeConfiguration : IEntityTypeConfiguration<UserRoleDomain>
    {
        public void Configure(EntityTypeBuilder<UserRoleDomain> builder)
        {
            builder.HasKey(e => new { e.UserId, e.Role });
        }
    }
}

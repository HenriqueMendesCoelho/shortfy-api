using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using suavesabor_api.User.Domain;

namespace suavesabor_api.src.Application.Data.TypeConfiguration
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<UserDomain>
    {
        public void Configure(EntityTypeBuilder<UserDomain> builder)
        {
            builder.HasIndex(e => e.Email).IsUnique();
        }
    }
}

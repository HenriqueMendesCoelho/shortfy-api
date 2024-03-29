using Microsoft.EntityFrameworkCore;
using suavesabor_api.src.Application.Data.TypeConfiguration;
using suavesabor_api.User.Domain;

namespace suavesabor_api.src.Application.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<UserDomain> User { get; set; }
        public DbSet<UserRoleDomain> UserRole { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserTypeConfiguration().Configure(modelBuilder.Entity<UserDomain>());
            new UserRoleTypeConfiguration().Configure(modelBuilder.Entity<UserRoleDomain>());
        }
    }
}

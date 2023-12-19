using Microsoft.EntityFrameworkCore;
using suavesabor_api.Application.Data.TypeConfiguration;
using suavesabor_api.User.Domain;

namespace suavesabor_api.Application.Data
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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace suavesabor_api.User.Domain
{
    [Table("UserRole")]
    public class UserRoleDomain
    {
        [Key]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public RoleDomain Role { get; set; }
        public UserDomain User { get; set; } = null!;
    }
}

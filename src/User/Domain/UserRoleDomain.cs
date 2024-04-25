using System.ComponentModel.DataAnnotations.Schema;

namespace shortfy_api.User.Domain
{
    [Table("UserRole")]
    public class UserRoleDomain
    {
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public RoleDomain Role { get; set; }
        public UserDomain User { get; set; } = null!;
    }
}

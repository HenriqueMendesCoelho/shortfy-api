using suavesabor_api.src.Application.Attributes;
using suavesabor_api.src.Application.Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace suavesabor_api.User.Domain
{
    [Table("User")]
    [IncludeRelatedEntities("Roles")]
    public class UserDomain : IEntity<Guid>
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public ICollection<UserRoleDomain> Roles { get; set; } = [];
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}

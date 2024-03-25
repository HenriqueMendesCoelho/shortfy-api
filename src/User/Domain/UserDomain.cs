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
        public required string Name { get; set; }
        [StringLength(100)]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<UserRoleDomain> Roles { get; set; } = [];
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}

using suavesabor_api.User.Domain;

namespace suavesabor_api.User.Endpoints.Dto
{
    public class UserResponseDto(UserDomain user)
    {
        public string Id { get; set; } = user.Id.ToString();
        public string? Name { get; set; } = user.Name;
        public string? Email { get; set; } = user.Email;
        public string? Password { get; set; } = user.Password;
        public DateTime? CreatedAt { get; set; } = user.CreatedAt;
    }
}

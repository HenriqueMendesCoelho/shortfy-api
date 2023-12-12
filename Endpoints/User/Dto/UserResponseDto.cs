using suavesabor_api.Domain.User;

namespace suavesabor_api.Endpoints.User.Dto
{
    public class UserResponseDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public UserResponseDto(UserDomain user) 
        {
            Id = user.Id.ToString();
            Name = user.Name;
            Email = user.Email;
        }
    }
}

using suavesabor_api.User.Domain;

namespace suavesabor_api.User.Endpoints.Dto
{
    public class UserRequestDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        public UserDomain ToDomain() { return new UserDomain { Name = Name, Email = Email, Password = Password }; }
    }
}

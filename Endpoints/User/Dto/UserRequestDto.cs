using suavesabor_api.Domain.User;

namespace suavesabor_api.Endpoints.User.Dto
{
    public class UserRequestDto
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";

        public UserDomain ToDomain() { return new UserDomain { Name = Name, Email = Email }; }
    }
}

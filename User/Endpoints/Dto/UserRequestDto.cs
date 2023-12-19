using FluentValidation;
using suavesabor_api.User.Domain;

namespace suavesabor_api.User.Endpoints.Dto
{
    public class UserRequestDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public UserDomain ToDomain() { return new UserDomain { Name = Name, Email = Email, Password = Password }; }
    }
}

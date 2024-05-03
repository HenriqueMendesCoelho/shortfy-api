using FluentValidation;

namespace shortfy_api.src.Authentication.Endpoints.Dto.Validators
{
    public class LoginOAuthRequestDtoValidator : AbstractValidator<LoginOAuthRequestDto>
    {
        public LoginOAuthRequestDtoValidator()
        {
            RuleFor(u => u.idToken).NotEmpty();
        }
    }
}

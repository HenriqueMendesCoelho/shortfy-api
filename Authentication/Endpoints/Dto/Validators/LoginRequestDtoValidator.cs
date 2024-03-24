using FluentValidation;

namespace suavesabor_api.Authentication.Endpoints.Dto.Validators
{
    public class LoginRequestDtoValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginRequestDtoValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(40)
                .WithMessage("Password must contain uppercase, lowercase, special character, number and at least 10 characters");
        }
    }
}

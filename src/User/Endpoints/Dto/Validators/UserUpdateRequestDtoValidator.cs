using FluentValidation;

namespace suavesabor_api.src.User.Endpoints.Dto.Validators
{
    public class UserUpdateRequestDtoValidator : AbstractValidator<UserUpdateRequestDto>
    {
        public UserUpdateRequestDtoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}

using FluentValidation;

namespace suavesabor_api.User.Endpoints.Dto.Validators
{
    public class UserRequestDtoValidator : AbstractValidator<UserRequestDto>
    {
        public UserRequestDtoValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(100);
            RuleFor(u => u.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(40)
                .Matches("(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%¨&*()_+-=´`{}^~:;?|'<,>.]).{10,40}")
                .WithMessage("Password must contain uppercase, lowercase, special character, number and at least 10 characters");
        }
    }
}
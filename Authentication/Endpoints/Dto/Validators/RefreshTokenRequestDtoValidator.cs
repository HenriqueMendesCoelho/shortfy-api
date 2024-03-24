using FluentValidation;

namespace suavesabor_api.Authentication.Endpoints.Dto.Validators
{
    public class RefreshTokenRequestDtoValidator : AbstractValidator<RefreshRequestDto>
    {
        public RefreshTokenRequestDtoValidator()
        {
            RuleFor(u => u.RefreshToken)
                .NotEmpty();
        }
    }
}

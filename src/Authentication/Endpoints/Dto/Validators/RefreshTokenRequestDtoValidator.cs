using FluentValidation;
using suavesabor_api.src.Authentication.Endpoints.Dto;

namespace suavesabor_api.src.Authentication.Endpoints.Dto.Validators
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

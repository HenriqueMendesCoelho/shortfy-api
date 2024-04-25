using FluentValidation;
using shortfy_api.src.Authentication.Endpoints.Dto;

namespace shortfy_api.src.Authentication.Endpoints.Dto.Validators
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

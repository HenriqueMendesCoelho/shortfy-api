using shortfy_api.src.Authentication.Domain;

namespace shortfy_api.src.Authentication.UseCase
{
    public interface IRefreshTokenUseCase
    {
        Task<LoginDomain> Execute(string refreshToken);
    }
}

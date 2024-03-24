using suavesabor_api.Authentication.Domain;

namespace suavesabor_api.Authentication.UseCase
{
    public interface IRefreshTokenUseCase
    {
        Task<LoginDomain> Execute(string refreshToken);
    }
}

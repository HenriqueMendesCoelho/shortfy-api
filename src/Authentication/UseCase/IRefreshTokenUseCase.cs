using suavesabor_api.src.Authentication.Domain;

namespace suavesabor_api.src.Authentication.UseCase
{
    public interface IRefreshTokenUseCase
    {
        Task<LoginDomain> Execute(string refreshToken);
    }
}

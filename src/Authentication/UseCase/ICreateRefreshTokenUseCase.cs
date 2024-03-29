using suavesabor_api.src.Authentication.Domain;

namespace suavesabor_api.src.Authentication.UseCase
{
    public interface ICreateRefreshTokenUseCase
    {
        RefreshTokenDomain Execute();
    }
}

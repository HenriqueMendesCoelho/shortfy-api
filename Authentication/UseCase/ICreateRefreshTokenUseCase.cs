using suavesabor_api.Authentication.Domain;

namespace suavesabor_api.Authentication.UseCase
{
    public interface ICreateRefreshTokenUseCase
    {
        RefreshTokenDomain Execute();
    }
}

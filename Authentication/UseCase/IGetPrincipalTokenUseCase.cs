using System.Security.Claims;

namespace suavesabor_api.Authentication.UseCase
{
    public interface IGetPrincipalTokenUseCase
    {
        ClaimsPrincipal GetPrincipal(string token);
    }
}

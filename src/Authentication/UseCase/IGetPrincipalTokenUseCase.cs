using System.Security.Claims;

namespace suavesabor_api.src.Authentication.UseCase
{
    public interface IGetPrincipalTokenUseCase
    {
        ClaimsPrincipal GetPrincipal(string token);
    }
}

using System.Security.Claims;

namespace shortfy_api.src.Authentication.UseCase
{
    public interface IGetPrincipalTokenUseCase
    {
        ClaimsPrincipal GetPrincipal(string token);
    }
}

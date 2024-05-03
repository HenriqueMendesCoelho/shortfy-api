using shortfy_api.src.Authentication.Domain;

namespace shortfy_api.src.Authentication.UseCase
{
    public interface ILoginGoogleUseCase
    {
        Task<LoginDomain> ExecuteAsync(string idToken);
    }
}

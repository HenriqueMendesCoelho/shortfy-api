using shortfy_api.src.Authentication.Domain;

namespace shortfy_api.src.Authentication.UseCase
{
    public interface ILoginMicrosoftUseCase
    {
        Task<LoginDomain> ExecuteAsync(string idToken);
    }
}

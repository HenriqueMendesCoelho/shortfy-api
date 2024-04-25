using shortfy_api.src.Authentication.Domain;

namespace shortfy_api.src.Authentication.UseCase
{
    public interface ILoginUseCase
    {
        Task<LoginDomain> Execute(string email, string passwordString);

        Task<LoginDomain> ExecuteFromGoogle(string idToken);
    }
}

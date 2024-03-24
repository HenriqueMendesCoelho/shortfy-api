using suavesabor_api.Authentication.Domain;

namespace suavesabor_api.Authentication.UseCase
{
    public interface ILoginUseCase
    {
        Task<LoginDomain> Execute(string email, string passwordString);
    }
}

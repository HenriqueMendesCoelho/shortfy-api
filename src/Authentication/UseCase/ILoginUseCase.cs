using suavesabor_api.src.Authentication.Domain;

namespace suavesabor_api.src.Authentication.UseCase
{
    public interface ILoginUseCase
    {
        Task<LoginDomain> Execute(string email, string passwordString);
    }
}

using suavesabor_api.Authentication.Domain;

namespace suavesabor_api.Authentication.UseCase
{
    public interface ICreateTokenUseCase
    {
        TokenDomain Execute(Guid id, string email, ICollection<string> roles);
    }
}

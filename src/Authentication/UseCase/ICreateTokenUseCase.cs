using suavesabor_api.src.Authentication.Domain;

namespace suavesabor_api.src.Authentication.UseCase
{
    public interface ICreateTokenUseCase
    {
        TokenDomain Execute(Guid id, string email, ICollection<string> roles);
    }
}

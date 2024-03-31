using suavesabor_api.src.Application.Domain;

namespace suavesabor_api.src.User.UseCase
{
    public interface IPromoteUserUseCase
    {
        Task<MessageDomain> Execute(Guid id, Guid idCurrentUser);
    }
}

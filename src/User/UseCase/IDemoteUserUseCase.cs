using suavesabor_api.src.Application.Domain;

namespace suavesabor_api.src.User.UseCase
{
    public interface IDemoteUserUseCase
    {
        Task<MessageDomain> Execute(Guid id, Guid idCurrentUser);
    }
}

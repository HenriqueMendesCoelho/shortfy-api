using shortfy_api.src.Application.Domain;

namespace shortfy_api.src.User.UseCase
{
    public interface IDemoteUserUseCase
    {
        Task<MessageDomain> Execute(Guid id, Guid idCurrentUser);
    }
}

using shortfy_api.src.Application.Domain;

namespace shortfy_api.src.User.UseCase
{
    public interface IPromoteUserUseCase
    {
        Task<MessageDomain> Execute(Guid id, Guid idCurrentUser);
    }
}

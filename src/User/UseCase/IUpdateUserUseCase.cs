using shortfy_api.User.Domain;

namespace shortfy_api.src.User.UseCase
{
    public interface IUpdateUserUseCase
    {
        Task<UserDomain> Execute(UserDomain user, Guid id, Guid idCurrentUser);
    }
}

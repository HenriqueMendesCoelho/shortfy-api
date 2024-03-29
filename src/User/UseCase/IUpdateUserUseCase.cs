using suavesabor_api.User.Domain;

namespace suavesabor_api.src.User.UseCase
{
    public interface IUpdateUserUseCase
    {
        Task<UserDomain> Execute(UserDomain user, Guid id, Guid idCurrentUser);
    }
}

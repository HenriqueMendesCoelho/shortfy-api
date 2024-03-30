using suavesabor_api.User.Domain;

namespace suavesabor_api.User.UseCase
{
    public interface ICreateUserUseCase
    {
        Task<UserDomain> Execute(UserDomain user, Guid? idCurrentUser);
    }
}

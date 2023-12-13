using suavesabor_api.User.Domain;

namespace suavesabor_api.User.UseCase
{
    public interface ICreateUserUseCase
    {
        Task<UserDomain> Create(UserDomain user);
    }
}

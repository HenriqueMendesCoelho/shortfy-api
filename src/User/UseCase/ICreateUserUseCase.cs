using shortfy_api.User.Domain;

namespace shortfy_api.User.UseCase
{
    public interface ICreateUserUseCase
    {
        Task<UserDomain> Execute(UserDomain user);
    }
}

using shortfy_api.src.Authentication.Domain;
using shortfy_api.User.Domain;

namespace shortfy_api.src.Authentication.UseCase
{
    public interface ICreateUserLoginUseCase
    {
        Task<LoginDomain> ExecuteAsync(UserDomain user);

        Task<LoginDomain> ExecuteCreateUserIfNotExistsAsync(string name, string email, string? password);
    }
}

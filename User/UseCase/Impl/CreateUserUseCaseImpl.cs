using suavesabor_api.Application.Util;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private static readonly UserRoleDomain USER_ROLE = new() { Role = RoleDomain.USER };

        private readonly IUserRepository _repository = repository;

        async public Task<UserDomain> Create(UserDomain user)
        {
            user.Roles = [USER_ROLE];
            user.CreatedAt = DateTime.UtcNow;
            user.Password = EncryptPassword(user.Password);

            return await _repository.Create(user);
        }

        private string EncryptPassword(string password)
        {
            return PasswordHasherUtil.HashPassword(password);
        }
    }
}

using suavesabor_api.src.Application.Util;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private static readonly UserRoleDomain ADMIN_ROLE = new() { Role = RoleDomain.ADMIN };
        private static readonly UserRoleDomain USER_ROLE = new() { Role = RoleDomain.USER };

        private readonly IUserRepository _repository = repository;

        async public Task<UserDomain> Create(UserDomain user)
        {

            user.Roles = await ExistsAnyUser() ? [USER_ROLE] : [ADMIN_ROLE, USER_ROLE];
            user.CreatedAt = DateTime.UtcNow;
            user.Password = EncryptPassword(user.Password);

            return await _repository.Create(user);
        }

        async private Task<bool> ExistsAnyUser()
        {
            var usuarios = await _repository.FindAll();
            if (usuarios is null || usuarios.Count == 0)
            {
                return false;
            }

            return true;
        }

        private string EncryptPassword(string password)
        {
            return PasswordHasherUtil.HashPassword(password);
        }
    }
}

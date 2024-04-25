using shortfy_api.src.Application.Util;
using shortfy_api.src.User.UseCase.Exceptions;
using shortfy_api.User.Domain;
using shortfy_api.User.Repository;

namespace shortfy_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<UserDomain> Execute(UserDomain user)
        {
            var userExists = await _repository.FindByEmail(user.Email);
            if (userExists is not null)
            {
                throw new EmailConflictException();
            }

            user.Roles = await GetUserRoles();
            user.CreatedAt = DateTime.UtcNow;
            if (user.Password is not null)
            {
                user.Password = PasswordHasherUtil.HashPassword(user.Password);
            }

            return await _repository.Create(user);
        }

        async private Task<List<UserRoleDomain>> GetUserRoles()
        {
            var existsAnyUser = await ExistsAnyUser();
            var adminRole = new UserRoleDomain() { Role = RoleDomain.ADMIN };
            var userRole = new UserRoleDomain() { Role = RoleDomain.USER };

            List<UserRoleDomain> roles = [userRole];
            if (existsAnyUser is false)
            {
                roles.Add(adminRole);
            }

            return roles;
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
    }
}

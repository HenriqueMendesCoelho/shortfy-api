using suavesabor_api.src.Application.Util;
using suavesabor_api.src.User.UseCase.Exceptions;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<UserDomain> Create(UserDomain user)
        {
            var userExists = await _repository.FindByEmail(user.Email);
            if (userExists is not null)
            {
                throw new EmailConflictException();
            }

            user.Roles = await GetUserRoles();
            user.CreatedAt = DateTime.UtcNow;
            user.Password = PasswordHasherUtil.HashPassword(user.Password);

            return await _repository.Create(user);
        }

        async private Task<List<UserRoleDomain>> GetUserRoles()
        {
            var adminRole = new UserRoleDomain() { Role = RoleDomain.ADMIN };
            var userRole = new UserRoleDomain() { Role = RoleDomain.USER };

            List<UserRoleDomain> roles = [userRole];
            if (await NotExistsAnyUser())
            {
                roles.Add(adminRole);
            }

            return roles;
        }

        async private Task<bool> NotExistsAnyUser()
        {
            var usuarios = await _repository.FindAll();
            if (usuarios is null || usuarios.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}

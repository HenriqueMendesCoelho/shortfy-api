using suavesabor_api.src.Application.Util;
using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.src.User.UseCase.Exceptions;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<UserDomain> Execute(UserDomain user, Guid? idCurrentUser)
        {
            var existsAnyUser = await ExistsAnyUser();
            var currentUser = idCurrentUser is not null ? await _repository.FindByID(idCurrentUser ?? Guid.Empty) : null;
            if (existsAnyUser && idCurrentUser is null)
            {
                throw new UserAcessNotAuthorizedException();
            }
            if (existsAnyUser && currentUser is null)
            {
                throw new UserAccessDeniedException();
            }
            if (existsAnyUser && currentUser is not null && !currentUser.Roles.Any(r => r.Role.ToString() == "ADMIN"))
            {
                throw new UserAccessDeniedException();
            }

            var userExists = await _repository.FindByEmail(user.Email);
            if (userExists is not null)
            {
                throw new EmailConflictException();
            }

            user.Roles = GetUserRoles(existsAnyUser);
            user.CreatedAt = DateTime.UtcNow;
            user.Password = PasswordHasherUtil.HashPassword(user.Password);

            return await _repository.Create(user);
        }

        private List<UserRoleDomain> GetUserRoles(bool existsAnyUser)
        {
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

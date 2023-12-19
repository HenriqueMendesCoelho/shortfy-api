using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;
using System.Data;

namespace suavesabor_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private static readonly UserRoleDomain USER_ROLE = new() { Role = RoleDomain.USER };

        private IUserRepository _repository = repository;

        async public Task<UserDomain> Create(UserDomain user)
        {
            user.Roles = [USER_ROLE];
            user.CreatedAt = DateTime.UtcNow;

            return await _repository.Create(user);
        }
    }
}

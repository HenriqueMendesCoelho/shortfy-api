using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class CreateUserUseCaseImpl(IUserRepository repository) : ICreateUserUseCase
    {
        private IUserRepository _repository = repository;
        async public Task<UserDomain> Create(UserDomain user)
        {
            user.CreatedAt = DateTime.Now;

            return await _repository.Create(user);
        }
    }
}

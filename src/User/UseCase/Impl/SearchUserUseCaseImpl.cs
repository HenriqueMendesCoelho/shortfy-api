using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class SearchUserUseCaseImpl(IUserRepository repository) : ISearchUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<List<UserDomain>> ListAll()
        {
            return await _repository.FindAll();
        }

        async public Task<UserDomain> FindByID(Guid id)
        {
            var user = await _repository.FindByID(id) ?? throw new UserNotFoundException();

            return user;
        }
    }
}

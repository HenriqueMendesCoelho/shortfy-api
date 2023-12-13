using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.User.UseCase.Impl
{
    public class SearchUserUseCaseImpl(IUserRepository repository) : ISearchUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<List<UserDomain>> listAll()
        {
            return await _repository.FindAll();
        }
    }
}

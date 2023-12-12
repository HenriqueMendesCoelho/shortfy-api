using suavesabor_api.Domain.User;
using suavesabor_api.Repository;

namespace suavesabor_api.UseCase.User.Impl
{
    public class SearchUserUseCaseImpl(IUserRepository repository) : ISearchUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        public List<UserDomain> listAll()
        {
            return _repository.FindAll();
        }
    }
}

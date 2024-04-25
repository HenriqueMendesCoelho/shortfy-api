using shortfy_api.src.Authentication.UseCase.Exceptions;
using shortfy_api.User.Domain;
using shortfy_api.User.Repository;

namespace shortfy_api.User.UseCase.Impl
{
    public class SearchUserUseCaseImpl(IUserRepository repository) : ISearchUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<List<UserDomain>> ListAll()
        {
            var result = await _repository.FindAll();
            result.Sort((u1, u2) =>
            {
                {
                    var nameComparison = u1.Name.CompareTo(u2.Name);
                    if (nameComparison != 0)
                    {
                        return nameComparison;
                    }

                    return u1.CreatedAt.CompareTo(u2.CreatedAt);
                }
            });

            return result;
        }

        async public Task<UserDomain> FindByID(Guid id)
        {
            var user = await _repository.FindByID(id) ?? throw new UserNotFoundException();

            return user;
        }
    }
}

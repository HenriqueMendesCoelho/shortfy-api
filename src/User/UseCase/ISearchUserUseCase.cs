using shortfy_api.User.Domain;

namespace shortfy_api.User.UseCase
{
    public interface ISearchUserUseCase
    {
        Task<List<UserDomain>> ListAll();

        Task<UserDomain> FindByID(Guid id);
    }
}

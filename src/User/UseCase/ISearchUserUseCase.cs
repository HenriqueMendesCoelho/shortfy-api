using suavesabor_api.User.Domain;

namespace suavesabor_api.User.UseCase
{
    public interface ISearchUserUseCase
    {
        Task<List<UserDomain>> ListAll();
    }
}

using suavesabor_api.Domain.User;

namespace suavesabor_api.UseCase.User
{
    public interface ISearchUserUseCase
    {
        List<UserDomain> listAll();
    }
}

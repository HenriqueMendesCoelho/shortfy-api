using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.src.User.UseCase.Exceptions;
using suavesabor_api.User.Repository;

namespace suavesabor_api.src.User.UseCase.Impl
{
    public class DeleteUserUseCaseImpl(IUserRepository userRepository) : IDeleteUserUseCase
    {
        private readonly IUserRepository _repository = userRepository;

        async public Task Execute(Guid id, Guid idCurrentUser)
        {
            var currentUser = await _repository.FindByID(idCurrentUser) ?? throw new UserNotFoundException();
            if (currentUser.Id == id)
            {
                throw new SelfDeletionNotAllowedException();
            }

            _ = await _repository.DeleteByID(id);
        }
    }
}

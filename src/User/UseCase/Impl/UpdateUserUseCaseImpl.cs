using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.src.User.UseCase.Impl
{
    public class UpdateUserUseCaseImpl(IUserRepository repository) : IUpdateUserUseCase
    {
        private readonly IUserRepository _repository = repository;

        async public Task<UserDomain> Execute(UserDomain user, Guid id, Guid idCurrentUser)
        {
            var currentUser = await _repository.FindByID(idCurrentUser) ?? throw new UserNotFoundException();
            var userToUpdate = await _repository.FindByID(id) ?? throw new UserNotFoundException();

            var isAdm = currentUser.Roles.Any(r => r.Role.ToString() == "ADMIN");
            if (currentUser.Id != userToUpdate.Id && !isAdm)
            {
                throw new UserAccessDeniedException();
            }

            userToUpdate.Name = user.Name;
            userToUpdate.Email = user.Email;

            var userUpdated = await _repository.Update(userToUpdate) ?? throw new UserNotFoundException();

            return userUpdated;
        }
    }
}

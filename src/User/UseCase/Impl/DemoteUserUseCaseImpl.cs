using suavesabor_api.src.Application.Domain;
using suavesabor_api.src.Authentication.UseCase.Exceptions;
using suavesabor_api.src.User.UseCase.Exceptions;
using suavesabor_api.User.Domain;
using suavesabor_api.User.Repository;

namespace suavesabor_api.src.User.UseCase.Impl
{
    public class DemoteUserUseCaseImpl(IUserRepository repository) : IDemoteUserUseCase
    {

        private readonly IUserRepository _repository = repository;

        async public Task<MessageDomain> Execute(Guid id, Guid idCurrentUser)
        {
            if (id.Equals(idCurrentUser))
            {
                throw new SelfDemotionAttemptException();
            }

            var user = await _repository.FindByID(id) ?? throw new UserNotFoundException();

            if (!user.Roles.Any(r => r.Role.ToString() == "ADMIN"))
            {
                throw new NonAdminDemotionAttemptException();
            }

            var adminRole = new UserRoleDomain() { Role = RoleDomain.ADMIN };
            user.Roles = user.Roles.Where(r => r.Role.ToString() != "ADMIN").ToList();

            await _repository.Update(user);

            return new MessageDomain() { Code = 200, Message = "User demoted" };
        }
    }
}

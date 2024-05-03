using shortfy_api.src.Authentication.Domain;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using shortfy_api.User.Domain;
using shortfy_api.User.Repository;
using shortfy_api.User.UseCase;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class CreateUserLoginUseCaseImpl(IUserRepository userRepository,
        ICreateRefreshTokenUseCase createRefreshTokenUseCase,
        ICreateTokenUseCase createTokenUseCase,
        ICreateUserUseCase createUserUseCase) : ICreateUserLoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICreateRefreshTokenUseCase _createRefreshTokenUseCase = createRefreshTokenUseCase;
        private readonly ICreateTokenUseCase _createTokenUseCase = createTokenUseCase;
        private readonly ICreateUserUseCase _createUserUseCase = createUserUseCase;

        public async Task<LoginDomain> ExecuteAsync(UserDomain user)
        {
            var roles = GetUserRoles(user);

            return await createResponseAndUpdateUserAsync(user, roles);
        }

        public async Task<LoginDomain> ExecuteCreateUserIfNotExistsAsync(string name, string email, string? password)
        {
            try
            {
                var (user, roles) = await findUserAndRolesAsync(email);
                return await createResponseAndUpdateUserAsync(user, roles);
            }
            catch (UserNotFoundException)
            {
                var user = await _createUserUseCase.Execute(new UserDomain { Name = name, Email = email, Password = password });
                return await createResponseAndUpdateUserAsync(user, GetUserRoles(user));
            }
        }

        private async Task<(UserDomain user, List<string> roles)> findUserAndRolesAsync(string email)
        {
            var user = await _userRepository.FindByEmail(email) ?? throw new UserNotFoundException();
            var roles = GetUserRoles(user);

            return (user, roles);
        }

        private async Task<LoginDomain> createResponseAndUpdateUserAsync(UserDomain user, List<string> roles)
        {
            var acessToken = _createTokenUseCase.Execute(user.Id, user.Email, roles);
            var newRefreshToken = _createRefreshTokenUseCase.Execute();

            user.RefreshToken = newRefreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = newRefreshToken.Expiration;
            await _userRepository.Update(user);

            return new LoginDomain { Authenticated = true, AccessToken = acessToken.AccessToken, ExpirationAcessToken = acessToken.Expiration, RefreshToken = newRefreshToken.RefreshToken, ExpirationRefreshToken = newRefreshToken.Expiration };
        }

        private List<string> GetUserRoles(UserDomain user)
        {
            return user.Roles.Select(r => r.Role.ToString() ?? string.Empty).ToList();
        }
    }
}

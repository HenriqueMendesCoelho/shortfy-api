using shortfy_api.src.Application.Util;
using shortfy_api.src.Authentication.Domain;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using shortfy_api.User.Repository;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class LoginUseCaseImpl(IUserRepository userRepository, ICreateRefreshTokenUseCase createRefreshTokenUseCase,
        ICreateTokenUseCase createTokenUseCase) : ILoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICreateRefreshTokenUseCase _createRefreshTokenUseCase = createRefreshTokenUseCase;
        private readonly ICreateTokenUseCase _createTokenUseCase = createTokenUseCase;

        async public Task<LoginDomain> Execute(string email, string passwordString)
        {
            var user = await _userRepository.FindByEmail(email) ?? throw new UserNotFoundException();

            if (PasswordHasherUtil.VerifyPassword(passwordString, user.Password) is not true) throw new UserAccessDeniedException();
            var newRefreshToken = _createRefreshTokenUseCase.Execute();
            var roles = user.Roles.Select(r => r.Role.ToString() ?? string.Empty).ToList();
            var acessToken = _createTokenUseCase.Execute(user.Id, user.Email, roles);

            user.RefreshToken = newRefreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = newRefreshToken.Expiration;
            await _userRepository.Update(user);

            return new LoginDomain { Authenticated = true, AccessToken = acessToken.AccessToken, ExpirationAcessToken = acessToken.Expiration, RefreshToken = newRefreshToken.RefreshToken, ExpirationRefreshToken = newRefreshToken.Expiration };
        }
    }
}

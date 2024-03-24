using Microsoft.IdentityModel.Tokens;
using suavesabor_api.Authentication.Domain;
using suavesabor_api.Authentication.UseCase.Exception;
using suavesabor_api.User.Repository;

namespace suavesabor_api.Authentication.UseCase.Impl
{
    public class RefreshTokenUseCaseImpl(ICreateRefreshTokenUseCase createRefreshTokenUseCase, ICreateTokenUseCase createTokenUseCase,
        IUserRepository userRepository) : IRefreshTokenUseCase
    {
        private readonly ICreateRefreshTokenUseCase _createRefreshTokenUseCase = createRefreshTokenUseCase;
        private readonly ICreateTokenUseCase _createTokenUseCase = createTokenUseCase;
        private readonly IUserRepository _userRepository = userRepository;

        async public Task<LoginDomain> Execute(string refreshToken)
        {
            var user = await _userRepository.FindByRefreshToken(refreshToken) ?? throw new UserNotFoundException();
            if (string.IsNullOrEmpty(user.RefreshToken) || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var newRefreshToken = _createRefreshTokenUseCase.Execute();
            var roles = user.Roles.Select(r => r.ToString() ?? throw new SecurityTokenException("Invalid token")).ToList();
            var acessToken = _createTokenUseCase.Execute(user.Id, user.Email, roles);

            user.RefreshToken = newRefreshToken.RefreshToken;
            user.RefreshTokenExpiryTime = newRefreshToken.Expiration;
            await _userRepository.Update(user);

            return new LoginDomain { Authenticated = true, AccessToken = acessToken.AccessToken, ExpirationAcessToken = acessToken.Expiration, RefreshToken = newRefreshToken.RefreshToken, ExpirationRefreshToken = newRefreshToken.Expiration };
        }
    }
}

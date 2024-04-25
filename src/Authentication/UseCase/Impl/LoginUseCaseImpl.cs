using Google.Apis.Auth;
using shortfy_api.src.Application.Configuration;
using shortfy_api.src.Application.Util;
using shortfy_api.src.Authentication.Domain;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using shortfy_api.User.Domain;
using shortfy_api.User.Repository;
using shortfy_api.User.UseCase;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class LoginUseCaseImpl(IUserRepository userRepository, ICreateRefreshTokenUseCase createRefreshTokenUseCase,
        ICreateTokenUseCase createTokenUseCase, ICreateUserUseCase createUserUseCase, GoogleConfiguration googleConfiguration) : ILoginUseCase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ICreateRefreshTokenUseCase _createRefreshTokenUseCase = createRefreshTokenUseCase;
        private readonly ICreateTokenUseCase _createTokenUseCase = createTokenUseCase;
        private readonly ICreateUserUseCase _createUserUseCase = createUserUseCase;
        private readonly GoogleConfiguration _googleConfiguration = googleConfiguration;

        async public Task<LoginDomain> Execute(string email, string passwordString)
        {
            var (user, roles) = await findUserAndRoles(email);
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new UserAccessDeniedException();
            }
            if (PasswordHasherUtil.VerifyPassword(passwordString, user.Password) is not true)
            {
                throw new UserAccessDeniedException();
            }

            return await createResponseAndUpdateUser(user, roles);
        }

        async public Task<LoginDomain> ExecuteFromGoogle(string idToken)
        {
            try
            {
                var validationSettings = new ValidationSettings
                {
                    Audience = [_googleConfiguration.ClientId]
                };
                Payload payload = await ValidateAsync(idToken, validationSettings);

                try
                {
                    var (user, roles) = await findUserAndRoles(payload.Email);
                    return await createResponseAndUpdateUser(user, roles);
                }
                catch (UserNotFoundException)
                {
                    var user = await _createUserUseCase.Execute(new UserDomain { Name = payload.Name, Email = payload.Email, Password = null });
                    return await createResponseAndUpdateUser(user, GetUserRoles(user));
                }
            }
            catch (InvalidJwtException)
            {
                throw new UserAccessDeniedException();
            }
        }

        async private Task<(UserDomain user, List<string> roles)> findUserAndRoles(string email)
        {
            var user = await _userRepository.FindByEmail(email) ?? throw new UserNotFoundException();
            var roles = GetUserRoles(user);

            return (user, roles);
        }

        async private Task<LoginDomain> createResponseAndUpdateUser(UserDomain user, List<string> roles)
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

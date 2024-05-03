using Google.Apis.Auth;
using shortfy_api.src.Application.Configuration;
using shortfy_api.src.Authentication.Domain;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class LoginGoogleUseCaseImpl(ICreateUserLoginUseCase createUserLoginUseCase,
        GoogleConfiguration googleConfiguration,
        ILogger<LoginGoogleUseCaseImpl> logger) : ILoginGoogleUseCase
    {
        private readonly GoogleConfiguration _googleConfiguration = googleConfiguration;
        private readonly ICreateUserLoginUseCase _createUserLoginUseCase = createUserLoginUseCase;

        public async Task<LoginDomain> ExecuteAsync(string idToken)
        {
            try
            {
                var validationSettings = new ValidationSettings
                {
                    Audience = [_googleConfiguration.ClientId]
                };
                Payload payload = await ValidateAsync(idToken, validationSettings);

                return await _createUserLoginUseCase.ExecuteCreateUserIfNotExistsAsync(payload.Name, payload.Email, null);
            }
            catch (InvalidJwtException e)
            {
                logger.LogError(e.Message, e);
                throw new UserAccessDeniedException();
            }
        }
    }
}

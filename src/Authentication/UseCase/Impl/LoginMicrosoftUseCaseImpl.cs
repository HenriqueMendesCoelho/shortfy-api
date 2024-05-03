using Microsoft.IdentityModel.Tokens;
using shortfy_api.src.Application.Configuration;
using shortfy_api.src.Authentication.Domain;
using shortfy_api.src.Authentication.UseCase.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using static shortfy_api.src.Authentication.Domain.JWKSDomain;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class LoginMicrosoftUseCaseImpl(ICreateUserLoginUseCase createUserLoginUseCase,
        MicrosoftEntraIdConfiguration microsoftEntraIdConfiguration,
        ILogger<LoginMicrosoftUseCaseImpl> logger) : ILoginMicrosoftUseCase
    {
        private readonly ICreateUserLoginUseCase _createUserLoginUseCase = createUserLoginUseCase;
        private readonly MicrosoftEntraIdConfiguration _microsoftEntraIdConfiguration = microsoftEntraIdConfiguration;
        private static DateTime? lastUpdate;
        private static JWKSDomain? jWKS;


        public async Task<LoginDomain> ExecuteAsync(string idToken)
        {
            var validationParameters = await GetValidationParametersAsync(getKid(idToken), getTid(idToken));
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(idToken, validationParameters, out var securityToken);
                var (name, email) = GetUserNameAndEmailFromToken(idToken);

                return await _createUserLoginUseCase.ExecuteCreateUserIfNotExistsAsync(name, email, null);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message, e);
                throw new UserAccessDeniedException();
            }
        }

        private async Task<TokenValidationParameters> GetValidationParametersAsync(string kid, string tid)
        {
            string Issuer = $"https://login.microsoftonline.com/{tid}/v2.0";
            var jwk = await getSigningKeyAsync(kid);

            return new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidAudience = _microsoftEntraIdConfiguration.ClientId,
                ValidIssuer = Issuer,
                IssuerSigningKey = jwk
            };
        }

        private async Task<RsaSecurityKey> getSigningKeyAsync(string kid)
        {
            var jkw = await GetJWKAsync(kid);
            var modulus = jkw.N;
            var exponent = jkw.E;

            var rsaParameters = new RSAParameters
            {
                Modulus = Base64UrlEncoder.DecodeBytes(modulus),
                Exponent = Base64UrlEncoder.DecodeBytes(exponent)
            };
            var rsa = RSA.Create();
            rsa.ImportParameters(rsaParameters);
            var rsaSecurityKey = new RsaSecurityKey(rsa);

            return rsaSecurityKey;
        }

        private async Task<JWK> GetJWKAsync(string kid, bool secondAttempt = false)
        {
            if (jWKS is null || lastUpdate is null || DateTime.Now - lastUpdate >= TimeSpan.FromHours(12) || secondAttempt)
            {
                jWKS = await getLastJWKAsync(kid);
                lastUpdate = DateTime.Now;

                return findJWK(kid);
            }

            try
            {
                return findJWK(kid);
            }
            catch (UserAccessDeniedException)
            {
                if (!secondAttempt)
                {
                    return await GetJWKAsync(kid, true);
                }

                throw;
            }

        }

        private JWK findJWK(string kid)
        {
            var jwk = jWKS!.Keys.FirstOrDefault(k => k.Kid == kid);
            if (jwk is null)
            {
                throw new UserAccessDeniedException("JWK not found");
            }

            return jwk;
        }

        private async Task<JWKSDomain> getLastJWKAsync(string kid)
        {
            var keys = "https://login.microsoftonline.com/common/discovery/v2.0/keys";
            HttpClient client = new HttpClient();
            string response = await client.GetStringAsync(keys);
            var jwks = JsonSerializer.Deserialize<JWKSDomain>(response)!;
            logger.LogTrace("Updated jwks");

            return jwks;
        }

        private string getKid(string idToken)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var kid = jwtSecurityToken.Header.GetValueOrDefault("kid")?.ToString();
            if (kid is null)
            {
                throw new UserAccessDeniedException("kid not found");
            }

            return kid;
        }

        private string getTid(string idToken)
        {
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            var tid = jwtSecurityToken.Payload.GetValueOrDefault("tid")?.ToString();
            if (tid is null)
            {
                throw new UserAccessDeniedException("tid not found");
            }

            return tid;
        }

        private (string name, string email) GetUserNameAndEmailFromToken(string idToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var payload = tokenHandler.ReadJwtToken(idToken).Payload;

            var name = payload.GetValueOrDefault("name")?.ToString();
            var email = payload.GetValueOrDefault("preferred_username")?.ToString();
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
            {
                throw new UserAccessDeniedException("name or email not found in token");
            }

            return (name, email);
        }
    }
}

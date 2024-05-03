using shortfy_api.src.Application.Configuration;
using shortfy_api.src.Authentication.Domain;
using System.Security.Cryptography;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class CreateRefreshTokenUseCaseImpl(TokenConfiguration tokenConfiguration) : ICreateRefreshTokenUseCase
    {
        private readonly TokenConfiguration _tokenConfiguration = tokenConfiguration;

        public RefreshTokenDomain Execute()
        {
            var refreshToken = GenerateRefreshToken();
            var expires = DateTime.UtcNow.AddDays(_tokenConfiguration.DaysToExpireRefreshToken);

            return new RefreshTokenDomain { RefreshToken = refreshToken, Expiration = expires };
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var refreshToken = Convert.ToBase64String(randomNumber);

            return refreshToken[..^2];
        }
    }
}

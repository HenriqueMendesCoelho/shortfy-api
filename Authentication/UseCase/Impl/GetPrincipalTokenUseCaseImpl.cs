using Microsoft.IdentityModel.Tokens;
using suavesabor_api.Application.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace suavesabor_api.Authentication.UseCase.Impl
{
    public class GetPrincipalTokenUseCaseImpl(TokenConfiguration tokenConfiguration) : IGetPrincipalTokenUseCase
    {
        private readonly TokenConfiguration _tokenConfiguration = tokenConfiguration;

        public ClaimsPrincipal GetPrincipal(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _tokenConfiguration.Issuer,
                ValidAudience = _tokenConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (securityToken is not JwtSecurityToken || jwtSecurityToken is null
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha384, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}

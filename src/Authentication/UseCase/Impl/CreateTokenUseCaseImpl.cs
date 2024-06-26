﻿using Microsoft.IdentityModel.Tokens;
using shortfy_api.src.Application.Configuration;
using shortfy_api.src.Authentication.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace shortfy_api.src.Authentication.UseCase.Impl
{
    public class CreateTokenUseCaseImpl(TokenConfiguration tokenConfiguration) : ICreateTokenUseCase
    {
        private readonly TokenConfiguration _tokenConfiguration = tokenConfiguration;

        public TokenDomain Execute(Guid id, string email, ICollection<string> roles)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret));
            SigningCredentials signingCredentials = new(secretKey, SecurityAlgorithms.HmacSha384);
            var expires = DateTime.UtcNow.AddMinutes(_tokenConfiguration.MinutesToExpireToken);
            var claims = CreateClaims(id, email, roles, expires);

            var tokenOptions = new JwtSecurityToken(
                issuer: _tokenConfiguration.Issuer,
                audience: _tokenConfiguration.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDomain { AccessToken = accessToken, Expiration = expires };
        }

        public List<Claim> CreateClaims(Guid id, string email, ICollection<string> roles, DateTime expires)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim("id", id.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Sub, email),
                new(JwtRegisteredClaimNames.Aud, _tokenConfiguration.Audience),
                new(JwtRegisteredClaimNames.Exp, expires.ToString()),
                new("roles", string.Join(",", roles))
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}

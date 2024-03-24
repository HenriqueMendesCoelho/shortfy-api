using suavesabor_api.Authentication.Domain;

namespace suavesabor_api.Authentication.Endpoints.Dto
{
    public class LoginResponseDto(LoginDomain domain)
    {
        public bool Authenticated { get; set; } = domain.Authenticated;
        public required string AccessToken { get; set; } = domain.AccessToken;
        public DateTime ExpirationAcessToken { get; set; } = domain.ExpirationAcessToken;
        public required string RefreshToken { get; set; } = domain.RefreshToken;
        public DateTime ExpirationRefreshToken { get; set; } = domain.ExpirationRefreshToken;


    }
}

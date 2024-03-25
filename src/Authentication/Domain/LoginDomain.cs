namespace suavesabor_api.src.Authentication.Domain
{
    public class LoginDomain
    {
        public bool Authenticated { get; set; }
        public required string AccessToken { get; set; }
        public DateTime ExpirationAcessToken { get; set; }
        public required string RefreshToken { get; set; }
        public DateTime ExpirationRefreshToken { get; set; }
    }
}

namespace suavesabor_api.Authentication.Domain
{
    public class RefreshTokenDomain
    {
        public required string RefreshToken { get; set; }
        public required DateTime Expiration { get; set; }
    }
}

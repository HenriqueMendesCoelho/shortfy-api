namespace suavesabor_api.Authentication.Domain
{
    public class TokenDomain
    {
        public required string AccessToken { get; set; }
        public required DateTime Expiration { get; set; }
    }
}

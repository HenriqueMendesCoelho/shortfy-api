namespace shortfy_api.src.Application.Configuration
{
    public class TokenConfiguration
    {
        public const string ConfigurationProps = "TokenConfiguration";
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required string Secret { get; set; }
        public int MinutesToExpireToken { get; set; }
        public int DaysToExpireRefreshToken { get; set; }
    }
}

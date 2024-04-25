namespace shortfy_api.src.Application.Configuration
{
    public class GoogleConfiguration
    {
        public const string ConfigurationProps = "GoogleConfiguration";
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace shortfy_api.src.Authentication.Domain
{
    public class JWKSDomain
    {
        [JsonPropertyName("keys")]
        public List<JWK> Keys { get; set; } = [];

        public class JWK
        {
            [JsonPropertyName("kty")]
            public string Kty { get; set; } = string.Empty;
            [JsonPropertyName("kid")]
            public string Kid { get; set; } = string.Empty;
            [JsonPropertyName("n")]
            public string N { get; set; } = string.Empty;
            [JsonPropertyName("e")]
            public string E { get; set; } = string.Empty;
        }
    }
}

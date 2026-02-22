using System.Text.Json.Serialization;

namespace RecoilReworkServer.Models
{
    public class RecoilReworkServerConfig
    {
        [JsonPropertyName("enableLogging")]
        public bool EnableLogging { get; set; } = false;
    }
}

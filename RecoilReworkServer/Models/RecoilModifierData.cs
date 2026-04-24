using System.Text.Json.Serialization;

namespace RecoilReworkServer.Models
{
    public class RecoilModifierData
    {
        [JsonPropertyName("VerticalKickMultiplier")]
        public float VerticalKickMultiplier { get; set; } = 1f;
        
        [JsonPropertyName("HorizontalKickMultiplier")]
        public float HorizontalKickMultiplier { get; set; } = 1f;

        [JsonPropertyName("IsBullpup")]
        public bool IsBullpup { get; set; } = false;
    }
}

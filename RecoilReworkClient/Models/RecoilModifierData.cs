using Newtonsoft.Json;

namespace RecoilReworkClient.Models
{
    public class RecoilModifierData
    {
        [JsonProperty("VerticalKickMultiplier")]
        public float VerticalKickMultiplier { get; set; } = 1f;
        
        [JsonProperty("HorizontalKickMultiplier")]
        public float HorizontalKickMultiplier { get; set; } = 1f;
        
        [JsonProperty("IsBullpup")]
        public bool IsBullpup { get; set; } = false;
    }
}

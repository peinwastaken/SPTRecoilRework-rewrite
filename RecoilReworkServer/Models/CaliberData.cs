using System.Text.Json.Serialization;

namespace RecoilReworkServer.Models
{
    public class CaliberData
    {
        [JsonPropertyName("BaseVerticalKick")]
        public float? BaseVerticalKick { get; set; } = 100f;

        [JsonPropertyName("BaseHorizontalKick")]
        public float? BaseHorizontalKick { get; set; } = 40f;

        [JsonPropertyName("BaseRollKick")]
        public float? BaseRollKick { get; set; } = 200f;
        
        [JsonPropertyName("BaseBackwardsRecoil")]
        public float? BaseBackwardsRecoil { get; set; } = 4f;
    }
}

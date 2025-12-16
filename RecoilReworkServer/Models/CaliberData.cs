using System.Text.Json.Serialization;

namespace RecoilReworkServer.Models
{
    public class CaliberData
    {
        [JsonPropertyName("BaseVerticalKick")]
        public float BaseVerticalKick { get; set; }
        
        [JsonPropertyName("BaseHorizontalKick")]
        public float BaseHorizontalKick { get; set; }
        
        [JsonPropertyName("BaseRollKick")]
        public float BaseRollKick { get; set; }
    }
}

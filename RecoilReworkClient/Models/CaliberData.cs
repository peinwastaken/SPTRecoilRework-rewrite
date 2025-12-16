using Newtonsoft.Json;

namespace RecoilReworkClient.Models
{
    public class CaliberData
    {
        [JsonProperty("BaseVerticalKick")]
        public float BaseVerticalKick { get; set; }
        
        [JsonProperty("BaseHorizontalKick")]
        public float BaseHorizontalKick { get; set; }
        
        [JsonProperty("BaseRollKick")]
        public float BaseRollKick { get; set; }
    }
}

using Newtonsoft.Json;

namespace RecoilReworkClient.Models
{
    public class CaliberData
    {
        [JsonProperty("BaseVerticalKick")]
        public float BaseVerticalKick { get; set; } = 100f;

        [JsonProperty("BaseHorizontalKick")]
        public float BaseHorizontalKick { get; set; } = 40f;

        [JsonProperty("BaseRollKick")]
        public float BaseRollKick { get; set; } = 200f;

        [JsonProperty("BaseBackwardsRecoil")]
        public float BaseBackwardsRecoil { get; set; } = 4f;
    }
}

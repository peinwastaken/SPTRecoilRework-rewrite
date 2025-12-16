using System.Text.Json.Serialization;

namespace RecoilReworkServer.Data
{
    public class CaliberData
    {
        [JsonPropertyName("BaseVerticalRecoil")]
        public float BaseVerticalRecoil { get; set; }
        
        [JsonPropertyName("BaseHorizontalRecoil")]
        public float BaseHorizontalRecoil { get; set; }
        
        [JsonPropertyName("BaseVerticalKick")]
        public float BaseVerticalKick { get; set; }
        
        [JsonPropertyName("BaseHorizontalKick")]
        public float BaseHorizontalKick { get; set; }
        
        [JsonPropertyName("StockVerticalRecoilMult")]
        public float StockVerticalRecoilMult { get; set; }
        
        [JsonPropertyName("StockHorizontalRecoilMult")]
        public float StockHorizontalRecoilMult { get; set; }
        
        [JsonPropertyName("StockVerticalKickMult")]
        public float StockVerticalKickMult { get; set; }
        
        [JsonPropertyName("StockHorizontalKickMult")]
        public float StockHorizontalKickMult { get; set; }
    }
}

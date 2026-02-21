using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using System.Text.Json.Serialization;

namespace RecoilReworkServer.Models
{
    public class WeaponData
    {
        [JsonPropertyName("weaponId")]
        public MongoId WeaponId { get; set; } = "";

        [JsonPropertyName("weaponIds")]
        public List<MongoId> WeaponIds { get; set; } = [];
        
        [JsonPropertyName("weaponRecoilModifiers")]
        public required RecoilModifierData RecoilModifiers { get; set; }
        
        [JsonPropertyName("overrideProperties")]
        public required TemplateItemProperties OverrideProperties { get; set; }
    }
}

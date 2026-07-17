using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class SprayPenaltySettings
    {
        public static ConfigEntry<bool> EnableSprayPenalty { get; private set; }
        public static ConfigEntry<float> RecoilSprayPenaltyMult { get; private set; }
        public static ConfigEntry<float> RollSprayPenaltyMult { get; private set; }
        public static ConfigEntry<float> WeightToPenaltyRecoveryModifier { get; private set; }
        public static ConfigEntry<float> WeightToPenaltyModifier { get; private set; }
        public static ConfigEntry<float> MaxSprayPenaltyMult { get; private set; }
        public static ConfigEntry<float> CaliberEnergyToPenaltyModifier { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);
            
            EnableSprayPenalty = config.Bind(
                category,
                "Enable Spray Penalty",
                true,
                new ConfigDescription(
                    "Enables the weapon spray penalty mechanic, which increases horizontal spread based on how long the weapon has been fired for.",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );
            
            WeightToPenaltyRecoveryModifier = config.Bind(
                category,
                "Weight To Penalty Recovery Modifier",
                0.15f,
                new ConfigDescription(
                    "Changes the weapon weight to penalty recovery curve modifier",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 990, IsAdvanced = true }
                )
            );
            
            WeightToPenaltyModifier = config.Bind(
                category,
                "Weight To Penalty Modifier",
                0.04f,
                new ConfigDescription(
                    "Changes the weapon weight to penalty increase curve modifier",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 980, IsAdvanced = true }
                )
            );
            
            CaliberEnergyToPenaltyModifier = config.Bind(
                category,
                "Caliber Energy To Penalty Modifier",
                1f,
                new ConfigDescription(
                    "Changes the caliber energy to penalty per shot modifier",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 970, IsAdvanced = true }
                )
            );
            
            RecoilSprayPenaltyMult = config.Bind(
                category,
                "Recoil Spray Penalty Multiplier",
                3f,
                new ConfigDescription(
                    "Changes the amount of weapon deviation added by the spray penalty system.",
                    new AcceptableValueRange<float>(0, 10),
                    new ConfigurationManagerAttributes { Order = 960, IsAdvanced = true }
                )
            );
            
            RollSprayPenaltyMult = config.Bind(
                category,
                "Horizontal Spray Penalty Multiplier",
                3f,
                new ConfigDescription(
                    "Changes the amount of weapon rolling added by the spray penalty system.",
                    new AcceptableValueRange<float>(0, 10),
                    new ConfigurationManagerAttributes { Order = 950, IsAdvanced = true }
                )
            );
            
            MaxSprayPenaltyMult = config.Bind(
                category,
                "Maximum Spray Penalty Amount",
                2f,
                new ConfigDescription(
                    "Overall ceiling of the penalty system.",
                    null,
                    new ConfigurationManagerAttributes { Order = 940, IsAdvanced = true }
                )
            );
        }
    }
}

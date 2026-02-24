using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class SprayPenaltySettings
    {
        public static ConfigEntry<bool> EnableSprayPenalty { get; private set; }
        public static ConfigEntry<Vector2> PitchSprayPenaltyMult { get; private set; }
        public static ConfigEntry<Vector2> YawSprayPenaltyMult { get; private set; }
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
                    new ConfigurationManagerAttributes { Order = 950 }
                )
            );
            
            WeightToPenaltyModifier = config.Bind(
                category,
                "Weight To Penalty Modifier",
                0.04f,
                new ConfigDescription(
                    "Changes the weapon weight to penalty increase curve modifier",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 945 }
                )
            );
            
            CaliberEnergyToPenaltyModifier = config.Bind(
                category,
                "Caliber Energy To Penalty Modifier",
                1f,
                new ConfigDescription(
                    "Changes the caliber energy to penalty per shot modifier",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 944 }
                )
            );
            
            PitchSprayPenaltyMult = config.Bind(
                category,
                "Vertical Spray Penalty Multiplier",
                new Vector2(-1.5f, 3.5f),
                new ConfigDescription(
                    "Changes the maximum amount of vertical weapon deviation inflicted by the penalty system where X = minimum and Y = maximum multiplier at maximum penalty",
                    null,
                    new ConfigurationManagerAttributes { Order = 940 }
                )
            );
            
            YawSprayPenaltyMult = config.Bind(
                category,
                "Horizontal Spray Penalty Multiplier",
                new Vector2(-1.5f, 3.5f),
                new ConfigDescription(
                    "Changes the maximum amount of horizontal weapon deviation inflicted by the penalty system where X = minimum and Y = maximum multiplier at maximum penalty",
                    null,
                    new ConfigurationManagerAttributes { Order = 935 }
                )
            );
            
            MaxSprayPenaltyMult = config.Bind(
                category,
                "Maximum Spray Penalty Amount",
                1f,
                new ConfigDescription(
                    "Overall maximum multiplier of the penalty system.",
                    null,
                    new ConfigurationManagerAttributes { Order = 930 }
                )
            );
        }
    }
}

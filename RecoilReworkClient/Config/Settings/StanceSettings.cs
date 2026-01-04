using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class StanceSettings
    {
        public static ConfigEntry<float> StandPenaltyMult { get; private set;  }
        public static ConfigEntry<float> CrouchPenaltyMult { get; private set; }
        public static ConfigEntry<float> PronePenaltyMult { get; private set; }
        public static ConfigEntry<float> MountPenaltyMult { get; private set; }
        public static ConfigEntry<float> AimingPenaltyMult { get; private set; }
        public static ConfigEntry<float> HipfirePenaltyMult { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);

            StandPenaltyMult = config.Bind(
                category,
                "Standing Spray Penalty Multiplier",
                1f,
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );
            
            CrouchPenaltyMult = config.Bind(
                category,
                "Crouching Spray Penalty Multiplier",
                0.5f,
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 990 }
                )
            );
            
            PronePenaltyMult = config.Bind(
                category,
                "Prone Spray Penalty Multiplier",
                0.2f,
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 980 }
                )
            );
            
            MountPenaltyMult = config.Bind(
                category,
                "Mounted Spray Penalty Multiplier",
                0.5f,
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 970 }
                )
            );
            
            AimingPenaltyMult = config.Bind(
                category,
                "Aiming Spray Penalty Multiplier",
                1f,
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 960 }
                )
            );
            
            HipfirePenaltyMult = config.Bind(
                category,
                "Hipfire Spray Penalty Multiplier",
                2f,
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 950 }
                )
            );
        }
    }
}

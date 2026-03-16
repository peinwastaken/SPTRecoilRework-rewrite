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
                "Standing spray penalty multiplier",
                1f,
                new ConfigDescription(
                    "Standing spray penalty multiplier",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );
            
            CrouchPenaltyMult = config.Bind(
                category,
                "Crouching spray penalty multiplier",
                0.5f,
                new ConfigDescription(
                    "Crouching spray penalty multiplier",
                    null,
                    new ConfigurationManagerAttributes { Order = 990 }
                )
            );
            
            PronePenaltyMult = config.Bind(
                category,
                "Prone spray penalty multiplier",
                0.2f,
                new ConfigDescription(
                    "Prone spray penalty multiplier",
                    null,
                    new ConfigurationManagerAttributes { Order = 980 }
                )
            );
            
            MountPenaltyMult = config.Bind(
                category,
                "Mounted spray penalty multiplier",
                0.5f,
                new ConfigDescription(
                    "Mounted spray penalty multiplier",
                    null,
                    new ConfigurationManagerAttributes { Order = 970 }
                )
            );
            
            AimingPenaltyMult = config.Bind(
                category,
                "Aiming spray penalty multiplier",
                1f,
                new ConfigDescription(
                    "Aiming spray penalty multiplier",
                    null,
                    new ConfigurationManagerAttributes { Order = 960 }
                )
            );
            
            HipfirePenaltyMult = config.Bind(
                category,
                "Hipfire spray penalty multiplier",
                2f,
                new ConfigDescription(
                    "Hipfire (point.... fire...? who cares) spray penalty multiplier",
                    null,
                    new ConfigurationManagerAttributes { Order = 950 }
                )
            );
        }
    }
}

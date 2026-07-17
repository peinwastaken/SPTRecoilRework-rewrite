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
        public static ConfigEntry<float> StandAngleRecoilMult { get; private set; }
        public static ConfigEntry<float> CrouchAngleRecoilMult { get; private set; }
        public static ConfigEntry<float> ProneAngleRecoilMult { get; private set; }
        public static ConfigEntry<float> MountAngleRecoilMult { get; private set; }
        public static ConfigEntry<float> AimingAngleRecoilMult { get; private set; }
        public static ConfigEntry<float> HipfireAngleRecoilMult { get; private set; }

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

            StandAngleRecoilMult = config.Bind(
                category,
                "Standing angle recoil multiplier",
                1f,
                new ConfigDescription(
                    "Angular recoil multiplier while standing.",
                    null,
                    new ConfigurationManagerAttributes { Order = 940 }
                )
            );

            CrouchAngleRecoilMult = config.Bind(
                category,
                "Crouching angle recoil multiplier",
                0.5f,
                new ConfigDescription(
                    "Angular recoil multiplier while crouching.",
                    null,
                    new ConfigurationManagerAttributes { Order = 930 }
                )
            );

            ProneAngleRecoilMult = config.Bind(
                category,
                "Prone angle recoil multiplier",
                0.3f,
                new ConfigDescription(
                    "Angular recoil multiplier while prone.",
                    null,
                    new ConfigurationManagerAttributes { Order = 920 }
                )
            );

            MountAngleRecoilMult = config.Bind(
                category,
                "Mounted angle recoil multiplier",
                0f,
                new ConfigDescription(
                    "Angular recoil multiplier while mounted.",
                    null,
                    new ConfigurationManagerAttributes { Order = 910 }
                )
            );

            AimingAngleRecoilMult = config.Bind(
                category,
                "Aiming angle recoil multiplier",
                1f,
                new ConfigDescription(
                    "Angular recoil multiplier while aiming.",
                    null,
                    new ConfigurationManagerAttributes { Order = 900 }
                )
            );

            HipfireAngleRecoilMult = config.Bind(
                category,
                "Hipfire angle recoil multiplier",
                1f,
                new ConfigDescription(
                    "Angular recoil multiplier while hipfiring.",
                    null,
                    new ConfigurationManagerAttributes { Order = 890 }
                )
            );
        }
    }
}

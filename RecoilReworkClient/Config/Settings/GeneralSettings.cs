using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class GeneralSettings
    {
        public static ConfigEntry<bool> CrankRecoil { get; private set; }
        public static ConfigEntry<float> RifleCameraSnap { get; private set; }
        public static ConfigEntry<float> RifleCameraSnapNoStock { get; private set; }
        public static ConfigEntry<float> PistolCameraSnap { get; private set; }
        public static ConfigEntry<float> WeaponKickToAngleMult { get; private set; }
        public static ConfigEntry<Vector2> BackwardsToAngleRecoilModifier { get; private set; }
        public static ConfigEntry<float> AmmoModifierMult { get; private set; }
        public static ConfigEntry<float> WeightToPenaltyRecoveryModifier { get; private set; }
        public static ConfigEntry<float> WeightToPenaltyModifier { get; private set; }
        public static ConfigEntry<float> CaliberEnergyToPenaltyModifier { get; private set; }
        public static ConfigEntry<Vector2> SprayPenaltyMult { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);

            CrankRecoil = config.Bind(
                category,
                "Enable Crank Recoil",
                true,
                new ConfigDescription(
                    "Enables crank recoil. Disabling this makes weapons recoil away from the screen",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );

            RifleCameraSnap = config.Bind(
                category,
                "Rifle Camera Snap",
                3.5f,
                new ConfigDescription(
                    "Rifle camera snap speed, speed at which the camera's position adjusts to follow the weapon's rotational recoil",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 990 }
                )
            );
            
            RifleCameraSnapNoStock = config.Bind(
                category,
                "Rifle Camera Snap (Without Stock)",
                1.0f,
                new ConfigDescription(
                    "Same as above but when your weapon has no stock.",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 985 }
                )
            );
            
            PistolCameraSnap = config.Bind(
                category,
                "Pistol Camera Snap",
                0.2f,
                new ConfigDescription(
                    "Pistol camera snap speed, speed at which the camera's position adjusts to follow the weapon's rotational recoil",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 980 }
                )
            );
            
            WeaponKickToAngleMult = config.Bind(
                category,
                "Weapon Kick To Angle Mult",
                0.3f,
                new ConfigDescription(
                    "Weapon kick to angle mult, multiplier for the amount of angle recoil based on the amount of kick",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 970 }
                )
            );

            BackwardsToAngleRecoilModifier = config.Bind(
                category,
                "Backwards To Angle Recoil Modifier",
                new Vector2(0.5f, 0.5f),
                new ConfigDescription(
                    "does any of these variables even need a description? I could write some shit like \"changes the backwards angle recoil modifier value that calculates the...\" like it genuinely doesnt matter. you figure out what it does. it's related to caliber backwards recoil values and how theyre converted to additional recoil angle. good luck",
                    null,
                    new ConfigurationManagerAttributes() { Order = 965 }
                )
            );
            
            AmmoModifierMult = config.Bind(
                category,
                "Ammo Modifier Mult",
                1f,
                new ConfigDescription(
                    "Changes the amount that any ammo's recoil stat affects weapon kick and recoil",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 960 }
                )
            );

            WeightToPenaltyRecoveryModifier = config.Bind(
                category,
                "Weight To Penalty Recovery Speed Modifier",
                0.1f,
                new ConfigDescription(
                    "asddsa",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 950 }
                )
            );
            
            WeightToPenaltyModifier = config.Bind(
                category,
                "Weight To Penalty Modifier",
                0.5f,
                new ConfigDescription(
                    "asddsa",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 945 }
                )
            );
            
            CaliberEnergyToPenaltyModifier = config.Bind(
                category,
                "Caliber Energy To Penalty Modifier",
                1f,
                new ConfigDescription(
                    "asddsa",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 944 }
                )
            );
            
            SprayPenaltyMult = config.Bind(
                category,
                "Spray Penalty Multiplier",
                new Vector2(0.4f, 0.2f),
                new ConfigDescription(
                    "asddsa",
                    null,
                    new ConfigurationManagerAttributes { Order = 940 }
                )
            );
        }
    }
}

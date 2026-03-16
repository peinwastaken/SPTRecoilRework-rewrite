using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class BaseRecoilSettings
    {
        public static ConfigEntry<float> WeaponKickToAngleMult { get; private set; }
        public static ConfigEntry<Vector2> BackwardsToAngleRecoilModifier { get; private set; }
        public static ConfigEntry<Vector2> BaseKickMultiplier;
        public static ConfigEntry<Vector2> BaseAngleMultiplier;

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);
            
            WeaponKickToAngleMult = config.Bind(
                category,
                "Weapon Kick To Angle Mult",
                0.2f,
                new ConfigDescription(
                    "Weapon kick to angle recoil multiplier. Multiplier for the amount of base angle recoil based on the amount of caliber kick",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 970 }
                )
            );

            BackwardsToAngleRecoilModifier = config.Bind(
                category,
                "Backwards Force To Recoil Modifier",
                new Vector2(0.8f, 0.8f),
                new ConfigDescription(
                    "Changes the modifier for backwards caliber force converted to additional weapon angle recoil",
                    null,
                    new ConfigurationManagerAttributes() { Order = 965 }
                )
            );

            BaseKickMultiplier = config.Bind(
                category,
                "Global Recoil Kick Multiplier",
                new Vector2(1f, 1f),
                new ConfigDescription(
                    "Changes the weapon kick multiplier. Think of this like a global recoil multiplier as angle recoil is calculated from the caliber's kick value. Per shot kick multiplier can be changed below.",
                    null,
                    new ConfigurationManagerAttributes() { Order = 960 }
                )
            );
            
            BaseAngleMultiplier = config.Bind(
                category,
                "Global Recoil Angle Multiplier",
                new Vector2(0.5f, 0.5f),
                new ConfigDescription(
                    "Changes the weapon angle recoil multiplier",
                    null,
                    new ConfigurationManagerAttributes() { Order = 955 }
                )
            );
        }
    }
}

using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class OnShotSettings
    {
        public static ConfigEntry<float> AmmoModifierMult { get; private set; }
        public static ConfigEntry<Vector3> FinalWeaponKickMult { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);
            
            AmmoModifierMult = config.Bind(
                category,
                "Ammo Recoil Modifier Multiplier",
                1f,
                new ConfigDescription(
                    "Changes the amount that any ammo's recoil reduction stat affects weapon kick in percentages",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 960 }
                )
            );
            
            FinalWeaponKickMult = config.Bind(
                category,
                "Final Weapon Kick Multiplier",
                new Vector3(2.0f, -1.0f, 2.0f),
                new ConfigDescription(
                    "Final weapon kick multiplier independent of any recoil calculations",
                    null,
                    new ConfigurationManagerAttributes { Order = 920 }
                )
            );
        }
    }
}

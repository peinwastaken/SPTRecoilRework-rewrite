using BepInEx.Configuration;

namespace RecoilReworkClient.Config.Settings
{
    public static class GeneralSettings
    {
        public static ConfigEntry<bool> CrankRecoil { get; private set; }
        public static ConfigEntry<float> RifleCameraSnap { get; private set; }
        public static ConfigEntry<float> PistolCameraSnap { get; private set; }
        public static ConfigEntry<float> WeaponKickToAngleMult { get; private set; }

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
                1f,
                new ConfigDescription(
                    "Rifle camera snap speed, speed at which the camera's position adjusts to follow the weapon's rotational recoil",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 990}
                )
            );
            
            PistolCameraSnap = config.Bind(
                category,
                "Pistol Camera Snap",
                0.2f,
                new ConfigDescription(
                    "Pistol camera snap speed, speed at which the camera's position adjusts to follow the weapon's rotational recoil",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 980}
                )
            );
            
            WeaponKickToAngleMult = config.Bind(
                category,
                "Weapon Kick To Angle Mult",
                1.2f,
                new ConfigDescription(
                    "Weapon kick to angle mult, multiplier for the amount of angle recoil based on the amount of kick",
                    new AcceptableValueRange<float>(0.01f, 10f),
                    new ConfigurationManagerAttributes { Order = 970}
                )
            );
        }
    }
}

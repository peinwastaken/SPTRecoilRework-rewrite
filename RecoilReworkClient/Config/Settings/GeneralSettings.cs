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
        }
    }
}

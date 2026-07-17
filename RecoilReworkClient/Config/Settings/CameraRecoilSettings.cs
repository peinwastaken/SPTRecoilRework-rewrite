using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class CameraRecoilSettings
    {
        public static ConfigEntry<bool> EnableAngleFollow { get; private set; }
        public static ConfigEntry<float> CameraRecoilPitchMultiplier { get; private set; }
        public static ConfigEntry<float> CameraRecoilYawMultiplier { get; private set; }
        public static ConfigEntry<Vector3> CameraRecoilImpulseMultiplier { get; private set; }
        public static ConfigEntry<float> AngleSpringFrequency { get; private set; }
        public static ConfigEntry<float> AngleSpringDampingRatio { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);

            EnableAngleFollow = config.Bind(
                category,
                "Enable Camera Recoil",
                false,
                new ConfigDescription(
                    "Changes if the camera rotation should follow the angle recoil of the weapon.",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );

            CameraRecoilPitchMultiplier = config.Bind(
                category,
                "Camera Recoil Pitch Multiplier",
                0.15f,
                new ConfigDescription(
                    "Changes the amount of pitch recoil applied to the camera.",
                    new AcceptableValueRange<float>(-5f, 5f),
                    new ConfigurationManagerAttributes { Order = 990 }
                )
            );

            CameraRecoilYawMultiplier = config.Bind(
                category,
                "Camera Recoil Yaw Multiplier",
                0.1f,
                new ConfigDescription(
                    "Changes the amount of yaw recoil applied to the camera.",
                    new AcceptableValueRange<float>(-5f, 5f),
                    new ConfigurationManagerAttributes { Order = 980 }
                )
            );

            CameraRecoilImpulseMultiplier = config.Bind(
                category,
                "Camera Recoil Impulse Multiplier",
                Vector3.one,
                new ConfigDescription(
                    "Changes the camera angle recoil multiplier where X = pitch, Y = yaw, Z = roll.",
                    null,
                    new ConfigurationManagerAttributes { Order = 970 }
                )
            );

            AngleSpringFrequency = config.Bind(
                category,
                "Camera Recoil Spring Frequency",
                5f,
                new ConfigDescription(
                    "Changes the camera angle recoil spring frequency. Higher values make camera recoil faster.",
                    new AcceptableValueRange<float>(0.1f, 20f),
                    new ConfigurationManagerAttributes { Order = 960 }
                )
            );

            AngleSpringDampingRatio = config.Bind(
                category,
                "Camera Recoil Spring Damping Ratio",
                0.4f,
                new ConfigDescription(
                    "Changes the camera angle recoil spring damping ratio. Higher values cause less camera recoil bounce.",
                    new AcceptableValueRange<float>(0f, 2f),
                    new ConfigurationManagerAttributes { Order = 950 }
                )
            );
        }
    }
}

using BepInEx.Configuration;
using UnityEngine;

namespace RecoilReworkClient.Config.Settings
{
    public static class CameraRecoilSettings
    {
        public static ConfigEntry<Vector3> AngleImpulseMultiplier { get; private set; }
        public static ConfigEntry<float> AngleSpringFrequency { get; private set; }
        public static ConfigEntry<float> AngleSpringDampingRatio { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);

            AngleImpulseMultiplier = config.Bind(
                category,
                "Camera Angle Impulse Multiplier",
                Vector3.one,
                new ConfigDescription(
                    "Changes the camera angle recoil multiplier where X = pitch, Y = yaw, Z = roll.",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );

            AngleSpringFrequency = config.Bind(
                category,
                "Camera Angle Spring Frequency",
                5f,
                new ConfigDescription(
                    "Changes the camera angle recoil spring frequency. Higher values make camera recoil faster.",
                    new AcceptableValueRange<float>(0.1f, 20f),
                    new ConfigurationManagerAttributes { Order = 990 }
                )
            );

            AngleSpringDampingRatio = config.Bind(
                category,
                "Camera Angle Spring Damping Ratio",
                0.4f,
                new ConfigDescription(
                    "Changes the camera angle recoil spring damping ratio. Higher values cause less camera recoil bounce.",
                    new AcceptableValueRange<float>(0f, 2f),
                    new ConfigurationManagerAttributes { Order = 980 }
                )
            );
        }
    }
}

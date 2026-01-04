using BepInEx.Configuration;

namespace RecoilReworkClient.Config.Settings
{
    public static class WeaponKickSettings
    {
        public static ConfigEntry<float> FallbackKick { get; private set; }
        public static ConfigEntry<float> PistolKick { get; private set; }
        public static ConfigEntry<float> AssaultRifleKick { get; private set; }
        public static ConfigEntry<float> AssaultCarbineKick { get; private set; }
        public static ConfigEntry<float> ShotgunKick { get; private set; }
        public static ConfigEntry<float> MachineGunKick { get; private set; }
        public static ConfigEntry<float> SubMachineGunKick { get; private set; }
        public static ConfigEntry<float> SniperRifleKick { get; private set; }
        public static ConfigEntry<float> MarksmanRifleKick { get; private set; }
        public static ConfigEntry<float> GrenadeLauncherKick { get; private set; }
        public static ConfigEntry<float> SpecialWeaponKick { get; private set; }
        
        
        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);

            FallbackKick = config.Bind(
                category,
                "None Kick Multiplier",
                1.0f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for fallback / unknown weapon classes",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 1000 }
                )
            );

            PistolKick = config.Bind(
                category,
                "Pistol Kick Multiplier",
                3f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for pistols",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 990 }
                )
            );

            AssaultRifleKick = config.Bind(
                category,
                "Assault Rifle Kick Multiplier",
                1f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for assault rifles",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 980 }
                )
            );

            AssaultCarbineKick = config.Bind(
                category,
                "Assault Carbine Kick Multiplier",
                1f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for assault carbines",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 970 }
                )
            );

            SubMachineGunKick = config.Bind(
                category,
                "Submachine Gun Kick Multiplier",
                1f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for SMGs",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 960 }
                )
            );

            ShotgunKick = config.Bind(
                category,
                "Shotgun Kick Multiplier",
                2f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for shotguns",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 950 }
                )
            );

            MachineGunKick = config.Bind(
                category,
                "Machine Gun Kick Multiplier",
                1.3f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for machine guns",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 940 }
                )
            );

            MarksmanRifleKick = config.Bind(
                category,
                "Marksman Rifle Kick Multiplier",
                1.4f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for marksman rifles",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 930 }
                )
            );

            SniperRifleKick = config.Bind(
                category,
                "Sniper Rifle Kick Multiplier",
                1.4f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for sniper rifles",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 920 }
                )
            );

            GrenadeLauncherKick = config.Bind(
                category,
                "Grenade Launcher Kick Multiplier",
                2f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for grenade launchers",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 910 }
                )
            );

            SpecialWeaponKick = config.Bind(
                category,
                "Special Weapon Kick Multiplier",
                2f,
                new ConfigDescription(
                    "Weapon kick impulse multiplier for special weapons",
                    new AcceptableValueRange<float>(0.1f, 3f),
                    new ConfigurationManagerAttributes { Order = 900 }
                )
            );
        }
    }
}

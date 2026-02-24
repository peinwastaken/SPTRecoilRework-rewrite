using BepInEx.Configuration;

namespace RecoilReworkClient.Config.Settings
{
    public static class DebugSettings
    {
        public static ConfigEntry<bool> EnableLogging { get; private set; }
        public static ConfigEntry<bool> EnableDebugOverlay { get; private set; }

        public static void Bind(int order, string category, ConfigFile config)
        {
            category = Category.Format(order, category);

            EnableLogging = config.Bind(
                category,
                "Enable Debug Logging",
                false,
                new ConfigDescription(
                    "Enables debug logging in BepInEx console",
                    null,
                    new ConfigurationManagerAttributes { Order = 1000, IsAdvanced = true}
                )
            );
            
            EnableDebugOverlay = config.Bind(
                category,
                "Enable Debug Overlay",
                false,
                new ConfigDescription(
                    "Enables the debug overlay for the recoil controller.",
                    null,
                    new ConfigurationManagerAttributes { Order = 990, IsAdvanced = true}
                )
            );
        }
    }
}

using RecoilReworkServer.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;

namespace RecoilReworkServer.Helpers
{
    [Injectable(InjectionType.Singleton)]
    public class ConfigHelper
    {
        public bool EnableLogging { get; private set; }

        public ConfigHelper(ModHelper modHelper, ModPaths modPaths)
        {
            RecoilReworkServerConfig config = modHelper.GetJsonDataFromFile<RecoilReworkServerConfig>(modPaths.ConfigPath, "config.jsonc");

            EnableLogging = config.EnableLogging;
        }
    }
}

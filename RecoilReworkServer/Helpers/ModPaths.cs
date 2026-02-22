using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Helpers;
using System.Reflection;

namespace RecoilReworkServer.Helpers
{
    [Injectable(InjectionType.Singleton)]
    public class ModPaths
    {
        public string ModPath { get; private set; }
        public string ConfigPath { get; private set; }
        public string CaliberDataPath { get; private set; }
        public string WeaponDataPath { get; private set; }

        public ModPaths(ModHelper modHelper)
        {
            ModPath = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
            ConfigPath = Path.Combine(ModPath, "Config");
            CaliberDataPath = Path.Combine(ConfigPath, "CaliberData");
            WeaponDataPath = Path.Combine(ConfigPath, "WeaponData");
        }
    }
}

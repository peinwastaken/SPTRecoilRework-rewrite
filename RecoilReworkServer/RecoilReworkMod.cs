using RecoilReworkServer.Helpers;
using RecoilReworkServer.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Utils;
using System.Reflection;

namespace RecoilReworkServer
{
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
    public class RecoilReworkMod(ISptLogger<RecoilReworkMod> logger, ModHelper modHelper) : IOnLoad
    {
        public string ModPath => modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        public string DataPath => Path.Combine(ModPath, "Data");
        
        public List<string> RandomStrings = [];
        
        public Task OnLoad()
        {
            Globals.CaliberData = modHelper.GetJsonDataFromFile<Dictionary<string, CaliberData>>(DataPath, "caliberdata.jsonc");
            RandomStrings = modHelper.GetJsonDataFromFile<List<string>>(DataPath, "randomstrings.jsonc");
            
            logger.LogWithColor($"Successfully loaded Recoil Rework. Loaded data for {Globals.CaliberData.Count} calibers. {RandomStrings.GetRandom()}", LogTextColor.Magenta);
            
            return Task.CompletedTask;
        }
    }
}

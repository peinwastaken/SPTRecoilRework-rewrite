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
        private string ModPath => modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        private string ConfigPath => Path.Combine(ModPath, "Config");
        private string CustomCaliberDataPath => Path.Combine(ConfigPath, "CustomCaliberData");
        
        public Task OnLoad()
        {
            Dictionary<string, CaliberData> caliberData = modHelper.GetJsonDataFromFile<Dictionary<string, CaliberData>>(ConfigPath, "caliberdata.jsonc");
            
            List<string> customDataFiles = Directory.GetFiles(CustomCaliberDataPath).ToList();
            
            foreach (string file in customDataFiles)
            {
                string extension = Path.GetExtension(file);

                if (extension != ".json" && extension != ".jsonc")
                {
                    continue;
                }
                
                Dictionary<string, CaliberData> customCaliberData = modHelper.GetJsonDataFromFile<Dictionary<string, CaliberData>>(CustomCaliberDataPath, file);
                foreach (var data in customCaliberData)
                {
                    caliberData.Add(data.Key, data.Value);
                }
            }

            Globals.CaliberData = caliberData;
            
            List<string> randomStrings = modHelper.GetJsonDataFromFile<List<string>>(ConfigPath, "randomstrings.jsonc");
            logger.LogWithColor($"Successfully loaded Recoil Rework. Loaded data for {caliberData.Count} calibers. {randomStrings.GetRandom()}", LogTextColor.Magenta);
            
            return Task.CompletedTask;
        }
    }
}

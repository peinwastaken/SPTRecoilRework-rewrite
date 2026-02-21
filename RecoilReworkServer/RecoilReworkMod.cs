using RecoilReworkServer.Helpers;
using RecoilReworkServer.Models;
using SPTarkov.Common.Extensions;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using System.Reflection;
using ItemHelper = RecoilReworkServer.Helpers.ItemHelper;
using Path = System.IO.Path;

namespace RecoilReworkServer
{
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
    public class RecoilReworkMod(ISptLogger<RecoilReworkMod> logger, ModHelper modHelper, DatabaseService dbService, ItemHelper itemHelper) : IOnLoad
    {
        private string ModPath => modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
        private string ConfigPath => Path.Combine(ModPath, "Config");
        private string CustomCaliberDataPath => Path.Combine(ConfigPath, "CustomCaliberData");
        private string WeaponDataPath => Path.Combine(ConfigPath, "WeaponData");
        
        public Task OnLoad()
        {
            LoadCaliberData();
            LoadWeaponData();
         
            List<string> randomStrings = modHelper.GetJsonDataFromFile<List<string>>(ConfigPath, "randomstrings.jsonc");
            logger.LogWithColor($"Loaded data for {Globals.CaliberData.Count} calibers. {randomStrings.GetRandom()}", LogTextColor.Magenta);
            
            return Task.CompletedTask;
        }

        private void LoadWeaponData()
        {
            var items = dbService.GetItems();
            
            List<WeaponData> weaponData = [];
            List<string> weaponDataFiles = Directory.GetFiles(WeaponDataPath).ToList();

            foreach (string file in weaponDataFiles)
            {
                string extension = Path.GetExtension(file);

                if (extension != ".json" && extension != ".jsonc")
                {
                    continue;
                }

                WeaponData data = modHelper.GetJsonDataFromFile<WeaponData>(WeaponDataPath, file);

                if (!data.WeaponId.IsEmpty)
                {
                    itemHelper.AddRecoilModifierData(items[data.WeaponId], data.RecoilModifiers);
                    Globals.WeaponData.Add(data.WeaponId, data.RecoilModifiers);
                    
                    logger.LogWithColor($"Loaded data for {data.WeaponId}", LogTextColor.Magenta);
                }
                
                if (data.WeaponIds.Count != 0)
                {
                    foreach (MongoId id in data.WeaponIds)
                    {
                        itemHelper.AddRecoilModifierData(items[id], data.RecoilModifiers);
                        Globals.WeaponData.Add(id,  data.RecoilModifiers);
                        
                        logger.LogWithColor($"Loaded override properties for {id}", LogTextColor.Magenta);
                    }   
                }
            }
        }

        private void LoadCaliberData()
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
        }
    }
}

using RecoilReworkServer.Helpers;
using RecoilReworkServer.Models;
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
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 67)]
    public class RecoilReworkMod(ISptLogger<RecoilReworkMod> logger, ModHelper modHelper, DatabaseService dbService, ItemHelper itemHelper, LoadHelper loadHelper) : IOnLoad
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
            logger.LogWithColor($"Successfully loaded Recoil Rework 2.0! Loaded data for {Globals.CaliberData.Count} calibers and {Globals.WeaponData.Count} weapons. {randomStrings.GetRandom()}", LogTextColor.Magenta);
            
            return Task.CompletedTask;
        }

        private void LoadWeaponData()
        {
            var items = dbService.GetItems();
            var fullData = loadHelper.LoadAllFromDirectory<List<WeaponData>>(WeaponDataPath);

            foreach (var weaponData in fullData)
            {
                foreach (var data in weaponData)
                {
                    if (!data.WeaponId.IsEmpty)
                    {
                        if (items.TryGetValue(data.WeaponId, out var value))
                        {
                            itemHelper.AddRecoilModifierData(value, data.RecoilModifiers);
                            itemHelper.UpdateBaseItem(value, data.OverrideProperties);
                            Globals.WeaponData.Add(data.WeaponId, data.RecoilModifiers);
                    
                            logger.LogWithColor($"Loaded weapon data for {data.WeaponId} :-)", LogTextColor.Green);
                        }
                        else
                        {
                            logger.LogWithColor($"Failed to find weapon with {data.WeaponId}. This is usually fine to ignore.", LogTextColor.Yellow);
                        }
                        return;
                    }
                
                    if (data.WeaponIds.Count != 0)
                    {
                        foreach (MongoId id in data.WeaponIds)
                        {
                            if (items.TryGetValue(id, out var value))
                            {
                                itemHelper.AddRecoilModifierData(items[id], data.RecoilModifiers);
                                itemHelper.UpdateBaseItem(items[id], data.OverrideProperties);
                                Globals.WeaponData.Add(id,  data.RecoilModifiers);
                        
                                logger.LogWithColor($"Loaded weapon data for {id} :-)", LogTextColor.Green);
                            }
                            else
                            {
                                logger.LogWithColor($"Failed to find weapon with {id}. This is usually fine to ignore.", LogTextColor.Yellow);
                            }
                        }
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

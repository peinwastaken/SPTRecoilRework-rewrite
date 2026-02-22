using RecoilReworkServer.Helpers;
using RecoilReworkServer.Models;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Logging;
using SPTarkov.Server.Core.Services;
using ItemHelper = RecoilReworkServer.Helpers.ItemHelper;

namespace RecoilReworkServer
{
    [Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 67)]
    public class RecoilReworkMod(
        ModHelper modHelper,
        DatabaseService dbService,
        ItemHelper itemHelper,
        LoadHelper loadHelper,
        ModPaths modPaths,
        DebugLogger logger) : IOnLoad
    {
        public Task OnLoad()
        {
            LoadCaliberData();
            LoadWeaponData();
         
            List<string> randomStrings = modHelper.GetJsonDataFromFile<List<string>>(modPaths.ConfigPath, "randomstrings.jsonc");
            logger.Log($"Successfully loaded Recoil Rework! Loaded data for {Globals.GlobalData.CaliberData.Count} calibers and {Globals.GlobalData.WeaponData.Count} weapons. {randomStrings.GetRandom()}", LogTextColor.Magenta, true);
            
            return Task.CompletedTask;
        }

        private void LoadWeaponData()
        {
            var items = dbService.GetItems();
            var fullData = loadHelper.LoadAllFromDirectory<List<WeaponData>>(modPaths.WeaponDataPath);

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
                            Globals.GlobalData.WeaponData.Add(data.WeaponId, data.RecoilModifiers);
                    
                            logger.LogInfo($"Loaded weapon data for {data.WeaponId} :-)");
                        }
                        else
                        {
                            logger.LogWarning($"Failed to find weapon {data.WeaponId}. Can be ignored and won't cause any issues. Just a heads up.");
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
                                Globals.GlobalData.WeaponData.Add(id,  data.RecoilModifiers);
                        
                                logger.LogInfo($"Loaded weapon data for {id} :-)");
                            }
                            else
                            {
                                logger.LogWarning($"Failed to find weapon {id}. Can be ignored and won't cause any issues. Just a heads up.");
                            }
                        }
                    }
                }
            }
        }

        private void LoadCaliberData()
        {
            List<Dictionary<string, CaliberData>> allCaliberData = loadHelper.LoadAllFromDirectory<Dictionary<string, CaliberData>>(modPaths.CaliberDataPath);
            Dictionary<string, CaliberData> loadedData = [];

            foreach (var caliberData in allCaliberData)
            {
                foreach (var data in caliberData)
                {
                    logger.LogInfo($"[Recoil Rework] Loaded caliber data for {data.Key} :-)");
                    loadedData.Add(data.Key, data.Value);
                }
            }

            Globals.GlobalData.CaliberData = loadedData;
        }
    }
}

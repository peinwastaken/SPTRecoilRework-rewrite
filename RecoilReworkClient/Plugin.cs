using BepInEx;
using BepInEx.Logging;
using RecoilReworkClient.Config;
using RecoilReworkClient.Config.Settings;
using RecoilReworkClient.Helpers;
using RecoilReworkClient.Models;
using RecoilReworkClient.Patches;
using System;
using System.Collections.Generic;

namespace RecoilReworkClient
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        public static Dictionary<string, CaliberData> CaliberData;
        public static Dictionary<string, RecoilModifierData> RecoilModifierData;

        private void Awake()
        {
            Logger = base.Logger;

            try
            {
                EnablePatches();
                FetchData();
                BindConfigs();
            
                Logger.LogInfo($"Loaded Recoil Rework {MyPluginInfo.PLUGIN_VERSION}!");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
        }

        private void EnablePatches()
        {
            new ComplexRotationPatch().Enable();
            new RecalculateRecoilParamsPatch().Enable();
            new SetStableModePatch().Enable();
            new ShotEffectorProcessPatch().Enable();
            new PlayerInitPatch().Enable();
            new LerpCameraPatch().Enable();
            //new ApplyThirdPersonTransformationsPatch().Enable();
        }

        private void BindConfigs()
        {
            GeneralSettings.Bind(0, Category.General, Config);
            WeaponKickSettings.Bind(1, Category.WeaponKick, Config);
            StanceSettings.Bind(2, Category.Stance, Config);
        }

        private void FetchData()
        {
            CaliberData = RouteHelper.FetchCaliberData();
            RecoilModifierData = RouteHelper.FetchModifierData();
            
            Logger.LogInfo($"Caliber data count: {CaliberData.Count}");
            foreach (var caliber in CaliberData)
            {
                Logger.LogInfo(caliber.Key);
            }
            
            Logger.LogInfo($"Recoil modifier data count: {RecoilModifierData.Count}");
        }
    }
}

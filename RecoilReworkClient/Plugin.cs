using BepInEx;
using BepInEx.Logging;
using GPUInstancer;
using RecoilReworkClient.Helpers;
using RecoilReworkClient.Models;
using RecoilReworkClient.Patches;
using System;
using System.Collections.Generic;

namespace RecoilReworkClient;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    public static Dictionary<string, CaliberData> CaliberData;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;

        try
        {
            EnablePatches();
            FetchData();
            
            Logger.LogInfo($"Loaded Recoil Rework {MyPluginInfo.PLUGIN_VERSION}!");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    private static void EnablePatches()
    {
        new ComplexRotationPatch().Enable();
        new RecalculateRecoilParamsPatch().Enable();
        new SetStableModePatch().Enable();
        new ShotEffectorProcessPatch().Enable();
        new PlayerInitPatch().Enable();
        new LerpCameraPatch().Enable();
    }

    private static void FetchData()
    {
        CaliberData = RouteHelper.FetchCaliberData();
        Plugin.Logger.LogInfo(CaliberData.Count);
        foreach (var caliber in CaliberData)
        {
            Plugin.Logger.LogInfo(caliber.Key);
        }
    }
}

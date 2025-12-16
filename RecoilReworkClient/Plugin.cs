using BepInEx;
using BepInEx.Logging;
using RecoilReworkClient.Patches;
using System;

namespace RecoilReworkClient;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    internal static new ManualLogSource Logger;

    private void Awake()
    {
        // Plugin startup logic
        Logger = base.Logger;

        try
        {
            EnablePatches();
            
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
}

using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class DisableLeftStanceFromHandsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LeftStanceController), nameof(LeftStanceController.DisableLeftStanceAnimFromHandsAction));
        }

        [PatchPrefix]
        private static bool PatchPrefix()
        {
            return false;
        }
    }
    
    public class DisableLeftStanceFromInventoryPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LeftStanceController), nameof(LeftStanceController.DisableLeftStanceAnimFromOpenInventory));
        }

        [PatchPrefix]
        private static bool PatchPrefix()
        {
            return false;
        }
    }
    
    public class DisableLeftStanceFromBodyPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(LeftStanceController), nameof(LeftStanceController.DisableLeftStanceAnimFromBodyAction));
        }

        [PatchPrefix]
        private static bool PatchPrefix()
        {
            return false;
        }
    }
}

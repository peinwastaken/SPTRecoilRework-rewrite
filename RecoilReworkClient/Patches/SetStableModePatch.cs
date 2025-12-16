using EFT.Animations;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class SetStableModePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(NewRotationRecoilProcess), nameof(NewRotationRecoilProcess.SetStableMode));
        }

        [PatchPrefix]
        private static bool Prefix(NewRotationRecoilProcess __instance)
        {
            return false;
        }
    }
}

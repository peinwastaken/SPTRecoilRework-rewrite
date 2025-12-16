using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class ShotEffectorProcessPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ShotEffector), nameof(ShotEffector.Process));
        }

        [PatchPrefix]
        private static bool Prefix(ShotEffector __instance)
        {
            RecoilController.Instance?.OnShoot();
            
            return false;
        }
    }
}

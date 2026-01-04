using EFT;
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
            Player.FirearmController fc = __instance.FirearmController;
            Player player = fc.GetComponent<Player>();
            if (!player.IsYourPlayer) return true;
            
            RecoilController.Instance?.OnShoot();
            
            return false;
        }
    }
}

using EFT;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class PlayerInitPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player), nameof(Player.Init));
        }

        [PatchPostfix]
        private static void Postfix(Player __instance)
        {
            if (__instance.IsYourPlayer)
            {
                __instance.gameObject.AddComponent<RecoilController>();
            }
        }
    }
}

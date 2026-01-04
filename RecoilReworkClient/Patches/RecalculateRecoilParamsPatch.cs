using EFT;
using EFT.Animations;
using EFT.Animations.NewRecoil;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class RecalculateRecoilParamsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(NewRecoilShotEffect), nameof(NewRecoilShotEffect.RecalculateRecoilParamsOnChangeWeapon));
        }

        [PatchPostfix]
        private static void Postfix(NewRecoilShotEffect __instance)
        {
            Player.FirearmController fc = __instance.FirearmController;
            if (fc == null) return;
            Player player = fc.GetComponent<Player>();
            if (player == null || !player.IsYourPlayer) return;
                
            ProceduralWeaponAnimation pwa = player.ProceduralWeaponAnimation;

            pwa.CrankRecoil = true;

            RecoilController.Instance?.RecalculateRecoilForces(player.ProceduralWeaponAnimation, fc.Weapon);
        }
    }
}

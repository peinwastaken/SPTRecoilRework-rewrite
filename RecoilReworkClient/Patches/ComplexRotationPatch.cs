using EFT;
using EFT.Animations;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace RecoilReworkClient.Patches
{
    public class ComplexRotationPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.ApplyComplexRotation));
        }

        [PatchPrefix]
        private static bool Prefix(ProceduralWeaponAnimation __instance)
        {
            Player.FirearmController firearmController = __instance.Shootingg.FirearmController;
            if (firearmController == null) return false;

            RecoilController.Instance?.ApplyComplexRotation(__instance);
            
            return false;
        }
    }
}

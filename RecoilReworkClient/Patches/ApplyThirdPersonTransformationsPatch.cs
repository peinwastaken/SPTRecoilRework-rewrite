using EFT.Animations;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class ApplyThirdPersonTransformationsPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(GClass910), nameof(GClass910.ApplyTransformations));
        }

        [PatchPrefix]
        public static bool PatchPrefix(GClass910 __instance, ProceduralWeaponAnimation pwa, float dt)
        {
            pwa.ZeroAdjustments();
            pwa.UpdateAimWeight(dt);
            pwa.BlendAnimatorPose(dt);
            pwa.ApplyPosition();
            pwa.ApplyComplexRotation(dt);
            pwa.ApplyTacticalReloadTransformations();
            pwa.AvoidObstacles();
            return false;
        } 
    }
}

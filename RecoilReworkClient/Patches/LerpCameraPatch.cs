using EFT.Animations;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace RecoilReworkClient.Patches
{
    public class LerpCameraPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.LerpCamera));
        }

        [PatchPostfix]
        private static void Postfix(ProceduralWeaponAnimation __instance)
        {
            if (RecoilController.Instance == null) return;
            
            RecoilController controller = RecoilController.Instance;
            
            Vector3 camPos = controller.CameraPositionSpring.Position;
            Vector3 camAngle = controller.CameraAngleSpring.Position;
            Vector3 recoilPos = controller.WeaponPositionSpring.Position;

            recoilPos.y *= controller.recoilBackPosCamVertOffsetMult;
            
            Vector3 finalCamPosOffset = camPos + recoilPos * controller.recoilPosInverseMult;
            Vector3 finalCamAngleOffset = camAngle;

            __instance.HandsContainer.CameraTransform.localPosition += finalCamPosOffset;
            __instance.HandsContainer.CameraTransform.localEulerAngles += finalCamAngleOffset;
        }
    }
}

using EFT.Animations;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Reflection;
using UnityEngine;

namespace RecoilReworkClient.Patches;

public class ProcessEffectorsPatch : ModulePatch
{
    protected override MethodBase GetTargetMethod()
    {
        return AccessTools.Method(typeof(ProceduralWeaponAnimation), nameof(ProceduralWeaponAnimation.ProcessEffectors));
    }

    [PatchPrefix]
    private static bool PatchPrefix(
        ProceduralWeaponAnimation __instance,
        float deltaTime,
        int nFixedFrames,
        Vector3 motion,
        Vector3 velocity,
        GInterface38 ____strategy)
    {
        __instance.MotionReact.Motion = motion;
        __instance.MotionReact.Velocity = velocity;
        ____strategy.ProcessEffectors(__instance, deltaTime, nFixedFrames);
        ____strategy.ApplyTransformations(__instance, deltaTime);
        ____strategy.ApplyCameraTransformations(__instance, deltaTime);
        return false;
    }
}

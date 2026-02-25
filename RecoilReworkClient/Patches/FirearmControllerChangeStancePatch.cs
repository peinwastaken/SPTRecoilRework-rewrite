using EFT;
using EFT.InventoryLogic;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class FirearmControllerChangeStancePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.Method(typeof(Player.FirearmController), nameof(Player.FirearmController.ChangeLeftStance));
        }

        [PatchPrefix]
        private static bool PatchPrefix(Player.FirearmController __instance)
        {
            if (__instance == null) return true;
            
            Player player = __instance.gameObject.GetComponent<Player>();
            if (player == null) return true;

            LeftStanceStateController stateController = player.GetComponent<LeftStanceStateController>();
            if (stateController == null) return true;

            Weapon weapon = __instance.Weapon;
            if (weapon.IsStationaryWeapon || weapon.BlockLeftStance || __instance.CurrentCompassState) return true;
            
            stateController.ToggleLeftStance();
            player.MovementContext.LeftStanceController.ToggleLeftStance();

            return false;
        }
    }
}

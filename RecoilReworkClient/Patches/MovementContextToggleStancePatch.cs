using EFT;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class MovementContextToggleStancePatch : ModulePatch
    {
        private static FieldInfo _playerField;
        
        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(MovementContext), "_player");
            return AccessTools.Method(typeof(MovementContext), nameof(MovementContext.method_24));
        }

        [PatchPrefix]
        private static bool PatchPrefix(MovementContext __instance, bool enable)
        {
            Player player = (Player)_playerField.GetValue(__instance);

            if (player.UsedSimplifiedSkeleton)
            {
                return false;
            }
            
            LeftStanceStateController stateController = player.GetComponent<LeftStanceStateController>();

            if (stateController != null)
            {
                stateController.SetLeftStance(enable);
                player.ProceduralWeaponAnimation.LeftStance = enable;
            }
            else
            {
                return true;
            }
            
            return false;
        }
    }
}

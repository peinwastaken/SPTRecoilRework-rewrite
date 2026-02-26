using EFT;
using HarmonyLib;
using RecoilReworkClient.Controllers;
using SPT.Reflection.Patching;
using System.Reflection;

namespace RecoilReworkClient.Patches
{
    public class MovementContextSetLeftStancePatch : ModulePatch
    {
        private static FieldInfo _playerField;
        
        protected override MethodBase GetTargetMethod()
        {
            _playerField = AccessTools.Field(typeof(MovementContext), "_player");
            return AccessTools.Method(typeof(MovementContext), nameof(MovementContext.method_25));
        }

        [PatchPrefix]
        private static bool PatchPrefix(MovementContext __instance)
        {
            Player player = (Player)_playerField.GetValue(__instance);
            if (player == null) return true;
            
            LeftStanceStateController controller = player.GetComponent<LeftStanceStateController>();
            if (controller == null) return true;

            return false;
        }
    }
}

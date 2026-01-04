using EFT;
using RecoilReworkClient.Config.Settings;
using System;

namespace RecoilReworkClient.Helpers
{
    public static class PlayerHelper
    {
        public static float GetStanceMultiplier(EPlayerPose pose)
        {
            return pose switch
            {
                EPlayerPose.Stand => StanceSettings.StandPenaltyMult.Value,
                EPlayerPose.Duck => StanceSettings.CrouchPenaltyMult.Value,
                EPlayerPose.Prone => StanceSettings.PronePenaltyMult.Value,
                _ => 1f
            };
        }
    }
}

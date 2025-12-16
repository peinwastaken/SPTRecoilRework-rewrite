using EFT.Animations;
using EFT.InventoryLogic;
using UnityEngine;

namespace RecoilReworkClient.Helpers
{
    public static class WeaponHelper
    {
        public static bool HasStock(this Weapon weapon)
        {
            foreach (Mod mod in weapon.Mods)
            {
                if (mod is StockItemClass)
                {
                    return true;
                }
            }

            return false;
        }

        public static Vector3 GetRecoilPivot(this ProceduralWeaponAnimation pwa)
        {
            if (pwa.IsMountedState)
            {
                if (!pwa.IsBipodUsed || pwa.IsVerticalMounting)
                {
                    return pwa.HandsContainer.MountingRotationCenter;
                }

                return pwa.HandsContainer.MountingRotationCenterBipods;
            }

            return pwa.HandsContainer.RecoilPivot;
        }
    }
}

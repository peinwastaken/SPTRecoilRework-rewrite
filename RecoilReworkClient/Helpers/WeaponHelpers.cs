using EFT.InventoryLogic;

namespace RecoilReworkClient.Helpers
{
    public static class WeaponHelpers
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
    }
}

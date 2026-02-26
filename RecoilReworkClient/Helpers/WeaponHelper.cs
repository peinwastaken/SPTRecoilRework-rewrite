using EFT.Animations;
using EFT.InventoryLogic;
using RecoilReworkClient.Enum;
using RecoilReworkClient.Models;
using System;
using UnityEngine;

namespace RecoilReworkClient.Helpers
{
    public static class WeaponHelper
    {
        public static string[] BullpupIds =
        [
            "5f2a9575926fd9352339381f", // rfb
            "62e7c4fba689e8c9c50dfc38", // aug a1
            "63171672192e68c5460cebc5", // aug a3
            "6718817435e3cfd9550d2c27", // aug a3 black
            "5c488a752e221602b412af63", // mdr 556
            "5dcbd56fdbd3d91b3e5468d5", // mdr 762
            "5cadfbf7ae92152ac412eeef", // ash-12
            "696ce75f73805e693401aba0", // armory msbs
            "696fe2ebcf7469bf3805173f", // armory msbs fde
            "66a47e98c486ec9d1af3a4da", // armory x95
            "66a544c956621d3364f6085e", // armory x95 fde,
            "66a545898022784400d6c836", // armory x95 od,
            "67f425638b8cbfdc0cd1b5f2", // armory x95 9mm,
            "6962f22fddc6698c6309b620", // armory f2000
            "6969867592a994a633084f70" // armory f2000 fde
        ];

        private static Type[] _recoilAffectingModTypes =
        [
            typeof(FlashHiderItemClass),
            typeof(MuzzleComboItemClass),
            typeof(SilencerItemClass),
            typeof(ForegripItemClass)
        ];

        public static bool HasStock(this Weapon weapon)
        {
            bool isPistol = weapon.IsPistol();
            bool hasStockSlot = false;
            
            foreach (Slot slot in weapon.AllSlots)
            {
                hasStockSlot = slot.ID.Contains("mod_stock");
                break;
            }
            
            // is weapon id in the bullpup array?
            if (weapon.IsBullpup())
            {
                return true;
            }
            
            // is weapon folded?
            if (weapon.Folded)
            {
                return false;
            }
            
            // does weapon have a stock or buffer tube equipped?
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
        
        public static EWeaponClass GetWeaponClass(this Weapon weapon)
        {
            string classId = weapon.WeapClass.ToLower();

            return classId switch
            {
                "assaultrifle" => EWeaponClass.AssaultRifle,
                "assaultcarbine" => EWeaponClass.AssaultCarbine,
                "pistol" => EWeaponClass.Pistol,
                "shotgun" => EWeaponClass.Shotgun,
                "sniperrifle" => EWeaponClass.SniperRifle,
                "machinegun" => EWeaponClass.MachineGun,
                "smg" => EWeaponClass.SubMachineGun,
                "marksmanrifle" => EWeaponClass.MarksmanRifle,
                "grenadelauncher" => EWeaponClass.GrenadeLauncher,
                "specialweapon" => EWeaponClass.SpecialWeapon,
                _ => EWeaponClass.None // hopefully this never happens
            };
        }
        
        public static bool IsPistol(this Weapon weapon)
        {
            return weapon.GetWeaponClass() == EWeaponClass.Pistol;
        }

        public static CaliberData GetCaliberData(this Weapon weapon)
        {
            Plugin.CaliberData.TryGetValue(weapon.Template.ammoCaliber, out CaliberData caliberData);
            
            return caliberData ?? Plugin.CaliberData["Fallback"];
        }

        public static RecoilModifierData GetModifierData(this Weapon weapon)
        {
            Plugin.RecoilModifierData.TryGetValue(weapon.StringTemplateId, out RecoilModifierData modifierData);

            return modifierData ?? new RecoilModifierData();
        }

        public static bool IsBullpup(this Weapon weapon)
        {
            return BullpupIds.ContainsKeyword(weapon.StringTemplateId);
        }

        public static float GetRecoilReduction(this Weapon weapon)
        {
            float recoilReduction = 0f;

            foreach (Slot slot in weapon.AllSlots)
            {
                Item containedItem = slot.ContainedItem;
                if (containedItem == null) continue;
                
                Type slotItemType = containedItem.GetType();
                bool slotHasRecoilReducingItem = _recoilAffectingModTypes.IndexOf(slotItemType) >= 0;
                
                if (slotHasRecoilReducingItem)
                {
                    Mod mod = slot.ContainedItem as Mod;
                    recoilReduction += mod.Recoil;
                }
            }
            
            return recoilReduction * 2 / 100f;
        }
    } 
}

using EFT.Animations;
using EFT.InventoryLogic;
using RecoilReworkClient.Controllers;
using RecoilReworkClient.Enum;
using RecoilReworkClient.Models;
using System;
using UnityEngine;

namespace RecoilReworkClient.Helpers
{
    public static class WeaponHelper
    {
        private static Type[] _recoilAffectingModTypes =
        [
            typeof(FlashHiderItemClass),
            typeof(MuzzleComboItemClass),
            typeof(SilencerItemClass),
            typeof(ForegripItemClass)
        ];

        public static bool HasStock(this Weapon weapon)
        {
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
            Plugin.RecoilModifierData.TryGetValue(weapon.StringTemplateId, out RecoilModifierData modifierData);

            return modifierData != null && modifierData.IsBullpup;
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

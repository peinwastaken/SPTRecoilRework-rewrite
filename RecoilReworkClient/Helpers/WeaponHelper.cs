using BepInEx.Configuration;
using EFT.Animations;
using EFT.InventoryLogic;
using RecoilReworkClient.Config.Settings;
using RecoilReworkClient.Enum;
using RecoilReworkClient.Models;
using System.Collections.Generic;
using UnityEngine;

namespace RecoilReworkClient.Helpers
{
    public static class WeaponHelper
    {
        public static Dictionary<EWeaponClass, ConfigEntry<float>> WeaponKickMultipliers = new Dictionary<EWeaponClass, ConfigEntry<float>>
        {
            {EWeaponClass.AssaultCarbine, WeaponKickSettings.AssaultCarbineKick},
            {EWeaponClass.AssaultRifle, WeaponKickSettings.AssaultRifleKick},
            {EWeaponClass.Pistol, WeaponKickSettings.PistolKick},
            {EWeaponClass.Shotgun, WeaponKickSettings.ShotgunKick},
            {EWeaponClass.MachineGun, WeaponKickSettings.MachineGunKick},
            {EWeaponClass.SubMachineGun, WeaponKickSettings.SubMachineGunKick},
            {EWeaponClass.SniperRifle, WeaponKickSettings.SniperRifleKick},
            {EWeaponClass.MarksmanRifle, WeaponKickSettings.MarksmanRifleKick},
            {EWeaponClass.GrenadeLauncher, WeaponKickSettings.GrenadeLauncherKick},
            {EWeaponClass.SpecialWeapon, WeaponKickSettings.SpecialWeaponKick},
        };

        public static bool HasStock(this Weapon weapon)
        {
            bool isPistol = weapon.IsPistol();
            bool hasStockSlot = false;
            
            foreach (Slot slot in weapon.AllSlots)
            {
                hasStockSlot = slot.ID.Contains("mod_stock");
                break;
            }
            
            // is weapon folded?
            if (weapon.Folded)
            {
                return false;
            }
            
            // assume were using a bullpup
            if (!isPistol && !hasStockSlot)
            {
                return true;
            }
            
            // does weapon have a stock equipped?
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

        public static float GetKickMultiplier(this Weapon weapon)
        {
            EWeaponClass weaponClass = weapon.GetWeaponClass();
            WeaponKickMultipliers.TryGetValue(weaponClass, out ConfigEntry<float> kickMult);
            
            return kickMult?.Value ?? 1f;
        }
    } 
}

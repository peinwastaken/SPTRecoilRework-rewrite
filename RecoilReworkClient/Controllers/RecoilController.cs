using BepInEx.Configuration;
using EFT;
using EFT.Animations;
using EFT.InventoryLogic;
using HarmonyLib;
using RecoilReworkClient.Config.Settings;
using RecoilReworkClient.Enum;
using RecoilReworkClient.Helpers;
using RecoilReworkClient.Models;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Spring = RecoilReworkClient.Physics.Spring;

namespace RecoilReworkClient.Controllers
{
    public class RecoilController : MonoBehaviour
    {
        public static RecoilController Instance;

        public Player player;
        public Weapon currentWeapon;
        public ProceduralWeaponAnimation proceduralWeaponAnimation;
        
        public Spring WeaponKickSpring;
        public Spring WeaponPositionSpring;
        public Spring WeaponAngleSpring;

        public Spring CameraPositionSpring;
        public Spring CameraAngleSpring;
        
        public float recoilPosInverseMult = 0.02f;
        public float recoilBackPosCamVertOffsetMult = 1f;

        public float KickPitchForce = 70f;
        public float KickYawForce = 30f;
        public float KickRollForce = 200f;

        public float PositionBackwardsForce = 2f;
        public float PositionUpwardsForce = 1f;
        public float PositionSidewaysForce = 0f;
        
        public float AnglePitchForce = 20f;
        public float AngleYawForce = 10f;
        public float AngleRollForce = 0f;
        
        public float AngleSprayPenalty = 0f;
        public float AngleRecoverySpeed = 8f;
        
        /*
         * x - cam pos right
         * y - cam pos up
         * z - cam pos forward
         */
        public Vector3 camPos = new Vector3(0, 0, 0);
        
        /*
         * x - cam ang pitch
         * y - cam ang yaw
         * z - cam ang roll
         */
        public Vector3 camAng = new Vector3(0, 0, 60);

        private Vector3 _tempPos = Vector3.zero;
        private Quaternion _tempRot = Quaternion.identity;
        private Quaternion _localRot = Quaternion.identity;
        private Quaternion _scopeRot = Quaternion.identity;

        private FieldInfo _adjustCollimatorsToTrajectoryField;
        private FieldInfo _hasPairOfIronsTransformsField;
        private FieldInfo _currentLosDeltaAngleField;
        private FieldInfo _losDeltaAngleField;
        private FieldInfo _targetScopeRotationField;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            _adjustCollimatorsToTrajectoryField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_adjustCollimatorsToTrajectory");
            _hasPairOfIronsTransformsField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_hasPairOfIronSightTransforms");
            _currentLosDeltaAngleField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_currentLineOfSightDeltaAngle");
            _losDeltaAngleField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_lineOfSightDeltaAngle");
            _targetScopeRotationField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_targetScopeRotation");

            Instance = this;
            player = GetComponent<Player>();
            proceduralWeaponAnimation = player.ProceduralWeaponAnimation;
            
            WeaponKickSpring = new Spring();
            WeaponPositionSpring = new Spring();
            WeaponAngleSpring = new Spring();
            
            CameraPositionSpring = new Spring();
            CameraAngleSpring = new Spring();

            WeaponKickSpring.DampingRatio = 0.4f;
            WeaponKickSpring.Frequency = 5f;
            WeaponKickSpring.Mass = 1f;
            WeaponKickSpring.Speed = 1f;

            WeaponPositionSpring.DampingRatio = 1f;
            WeaponPositionSpring.Frequency = 5f;
            WeaponPositionSpring.Mass = 1f;
            WeaponPositionSpring.Speed = 1f;

            WeaponAngleSpring.DampingRatio = 2f;
            WeaponAngleSpring.Frequency = 2f;
            WeaponAngleSpring.Mass = 1f;
            WeaponAngleSpring.Speed = 0.5f;

            CameraAngleSpring.DampingRatio = 0.2f;
            CameraAngleSpring.Frequency = 5f;
            CameraAngleSpring.Mass = 1f;
            CameraAngleSpring.Speed = 1.5f;

            CameraPositionSpring.DampingRatio = 1f;
            CameraPositionSpring.Frequency = 5f;
            CameraPositionSpring.Mass = 1f;
            CameraPositionSpring.Speed = 1f;
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            
            WeaponKickSpring.Update(dt);
            WeaponPositionSpring.Update(dt);
            WeaponAngleSpring.Update(dt);
            
            CameraPositionSpring.Update(dt);
            CameraAngleSpring.Update(dt);

            if (!proceduralWeaponAnimation.Shootingg.NewShotRecoil.AutoFireOn)
            {
                AngleSprayPenalty = Mathf.Lerp(AngleSprayPenalty, 0f, AngleRecoverySpeed * Time.fixedDeltaTime);
            }
        }

        public void RecalculateRecoilForces(ProceduralWeaponAnimation pwa, Weapon weapon)
        {
            currentWeapon = weapon;
            
            CaliberData caliberData = weapon.GetCaliberData();
            float kickMult = weapon.GetKickMultiplier();
            
            bool isPistol = weapon.IsPistol();
            bool hasStock = weapon.HasStock();
            float totalWeight = weapon.TotalWeight;
            
            float verticalKick = caliberData.BaseVerticalKick * kickMult;
            float horizontalKick = caliberData.BaseHorizontalKick * kickMult;
            float rollKick = caliberData.BaseRollKick * kickMult;

            float verticalAngle = verticalKick * GeneralSettings.WeaponKickToAngleMult.Value;
            float horizontalAngle = horizontalKick * GeneralSettings.WeaponKickToAngleMult.Value;
            
            float backwardsVertMult = 1f + GeneralSettings.BackwardsToAngleRecoilModifier.Value.x * Mathf.Pow(PositionBackwardsForce, 2f);
            float backwardsHorMult = 1f + GeneralSettings.BackwardsToAngleRecoilModifier.Value.y * Mathf.Pow(PositionBackwardsForce, 2f);
            verticalAngle *= backwardsVertMult;
            horizontalAngle *= backwardsHorMult;

            float backwardsRecoilClamped = Mathf.Clamp(caliberData.BaseBackwardsRecoil, 0f, 4f);
            
            // set caliber kick and recoil properties
            KickPitchForce = verticalKick;
            KickYawForce = horizontalKick;
            KickRollForce = rollKick;

            AnglePitchForce = verticalAngle + verticalAngle * weapon.RecoilDelta;
            AngleYawForce = horizontalAngle + horizontalAngle * weapon.RecoilDelta;
            
            PositionBackwardsForce = backwardsRecoilClamped;

            // calculate weapon kick damping
            float baseKickDamping = 0.3f;
            float kickWeightCoefficient = 0.02f;
            float finalKickDamping = baseKickDamping + (totalWeight * kickWeightCoefficient);
            WeaponKickSpring.DampingRatio = finalKickDamping;
            
            // calculate weapon spray penalty recovery speed
            float angleRecoverySpeed = Mathf.Max(8f - Mathf.Exp(GeneralSettings.WeightToPenaltyRecoveryModifier.Value * weapon.TotalWeight), 1f);
            AngleRecoverySpeed = angleRecoverySpeed;
            
            // other calculations go...
            // ...HERE!
            // end calculations.
            
            // recoil vertical mult based on if weapon is pistol
            if (isPistol)
            {
                recoilBackPosCamVertOffsetMult = 0f;
                PositionUpwardsForce = 0f;
                pwa.CameraSmoothRecoil = GeneralSettings.PistolCameraSnap.Value;
            }
            else
            {
                recoilBackPosCamVertOffsetMult = -weapon.StockRecoilDelta;
                PositionUpwardsForce = PositionBackwardsForce * 0.07f;
                float cameraSnap = hasStock ? GeneralSettings.RifleCameraSnap.Value : GeneralSettings.RifleCameraSnapNoStock.Value;
                pwa.CameraSmoothRecoil = cameraSnap;

                if (!hasStock)
                {
                    float caliberEnergy = new Vector2(caliberData.BaseVerticalKick, caliberData.BaseHorizontalKick).magnitude * 0.01f;
                    PositionUpwardsForce += caliberEnergy * 0.1f;
                }
            }
            
            pwa.CrankRecoil = GeneralSettings.CrankRecoil.Value;
        }
        
        public void OnShoot()
        {
            ProceduralWeaponAnimation pwa = player.ProceduralWeaponAnimation;
            EWeaponClass weaponClass = currentWeapon.GetWeaponClass();
            CaliberData caliberData = currentWeapon.GetCaliberData();
            ConfigEntry<float> kickMult = WeaponHelper.WeaponKickMultipliers[weaponClass];
            float stancePenaltyMult = PlayerHelper.GetStanceMultiplier(player.Pose);
            float adsPenaltyMult = pwa.IsAiming ? StanceSettings.AimingPenaltyMult.Value : StanceSettings.HipfirePenaltyMult.Value;
            float mountPenaltyMult = pwa.IsMountedState || pwa.IsVerticalMounting ? StanceSettings.MountPenaltyMult.Value : 1f;
            
            float ammoRecoilModifier = (currentWeapon.CurrentAmmoTemplate?.ammoRec ?? 0f) * GeneralSettings.AmmoModifierMult.Value;
            bool hasStock = currentWeapon.HasStock() && !currentWeapon.Folded;

            float modifiedRecoilPitch = Mathf.Max(0f, AnglePitchForce + ammoRecoilModifier);
            float modifiedRecoilYaw = Mathf.Max(0f, AngleYawForce + ammoRecoilModifier);
            float backwardsRecoilCamAngMult = 1f + 0.05f * Mathf.Pow(PositionBackwardsForce, 2);
            
            Vector3 recoilKickForce = new Vector3(-KickPitchForce, KickRollForce, KickYawForce);
            Vector3 recoilAngForce = new Vector3(-modifiedRecoilPitch, AngleRollForce, modifiedRecoilYaw);
            Vector3 recoilPosForce = new Vector3(PositionSidewaysForce, PositionBackwardsForce, -PositionUpwardsForce);
            
            Vector3 camAngForce = new Vector3(camAng.x, camAng.y, camAng.z) * backwardsRecoilCamAngMult;
            
            recoilKickForce.z *= Random.Range(-1f, 1f);
            recoilAngForce.z *= Random.Range(-1f, 1f);
            recoilPosForce.x *= Random.Range(-1f, 1f);

            recoilKickForce *= kickMult.Value;
            
            if (hasStock)
            {
                recoilKickForce.x *= 0.7f;
                recoilKickForce.y *= 0.7f;
                recoilKickForce.z *= 0.7f;
                
                recoilAngForce.x *= 0.7f;
                recoilAngForce.y *= 0.7f;
                recoilAngForce.z *= 0.7f;
                
                recoilPosForce.x *= 0.3f;
                recoilPosForce.y *= 0.7f;
                recoilPosForce.z *= 0.3f;
            }
            
            recoilKickForce *= Random.Range(0.9f, 1.1f);

            float pitchPenalty = 1f + AngleSprayPenalty * GeneralSettings.SprayPenaltyMult.Value.x;
            float yawPenalty = 1f + AngleSprayPenalty * GeneralSettings.SprayPenaltyMult.Value.y;
            
            recoilAngForce *= player.ProceduralWeaponAnimation.IsAiming ? 0.5f : 1f;
            recoilAngForce.x += recoilAngForce.x * Random.Range(-pitchPenalty, pitchPenalty);
            recoilAngForce.z += recoilAngForce.z * Random.Range(-yawPenalty, yawPenalty);
            
            WeaponKickSpring.ApplyImpulse(recoilKickForce);
            WeaponPositionSpring.ApplyImpulse(recoilPosForce);
            WeaponAngleSpring.ApplyImpulse(recoilAngForce);
            
            CameraPositionSpring.ApplyImpulse(camPos);
            CameraAngleSpring.ApplyImpulse(camAngForce);

            float caliberEnergy = new Vector2(caliberData.BaseVerticalKick, caliberData.BaseHorizontalKick).magnitude * 0.01f * GeneralSettings.CaliberEnergyToPenaltyModifier.Value;
            AngleSprayPenalty += (1f - Mathf.Exp(-currentWeapon.TotalWeight * GeneralSettings.WeightToPenaltyModifier.Value)) * caliberEnergy * stancePenaltyMult * adsPenaltyMult * mountPenaltyMult;
            AngleSprayPenalty = Mathf.Clamp(AngleSprayPenalty, 0f, 2f);
        }

        public void ApplyComplexRotation(ProceduralWeaponAnimation pwa)
        {
            float dt = Time.deltaTime;
            
            _tempPos = pwa.HandsContainer.WeaponRootAnim.position;
            _tempRot = pwa.HandsContainer.WeaponRootAnim.rotation;
            _localRot = Quaternion.identity;
            
            Vector3 handsRot = pwa.HandsContainer.HandsRotation.Get();
            Vector3 pivot = pwa.HandsContainer.WeaponRootAnim.TransformPoint(pwa.HandsContainer.RotationCenter);

            DeferredRotate(pwa, pivot, handsRot);

            // do recoil kick
            Vector3 recoilPivot = _tempPos + _tempRot * pwa.GetRecoilPivot();
            DeferredRotate(pwa, recoilPivot, WeaponKickSpring.Position);
            
            // do recoil angle
            DeferredRotate(pwa, recoilPivot, WeaponAngleSpring.Position);

            // do recoil position
            _tempPos += _tempRot * WeaponPositionSpring.Position;
            
            ApplyAimingAlignment(dt);
            
            // current scope rotation
            Quaternion targetScopeRotation = (Quaternion)_targetScopeRotationField.GetValue(pwa);
            _scopeRot = Quaternion.Lerp(_scopeRot, pwa.IsAiming ? targetScopeRotation : Quaternion.identity, pwa.CameraSmoothTime * pwa.AimingSpeed * dt);

            pwa.HandsContainer.WeaponRootAnim
            .SetPositionAndRotation(_tempPos, _tempRot * _scopeRot);
        }
        
        private void DeferredRotate(ProceduralWeaponAnimation pwa, Vector3 worldPivot, Vector3 rotation)
        {
            _localRot = Quaternion.Euler(rotation.x, 0f, rotation.z) * Quaternion.Euler(0f, rotation.y, 0f) * _localRot;

            Quaternion quat1 = pwa.HandsContainer.WeaponRootAnim.parent.rotation * _localRot;
            Quaternion quat2 = quat1 * Quaternion.Inverse(_tempRot);
            Vector3 vec1 = _tempPos - worldPivot;
            vec1 = quat2 * vec1;

            _tempRot = quat1;
            _tempPos = worldPivot + vec1;
        }
        
        private void ApplyAimingAlignment(float dt)
        {
            ProceduralWeaponAnimation pwa = player.ProceduralWeaponAnimation;
            bool adjustCollimatorsToTrajectory = (bool)_adjustCollimatorsToTrajectoryField.GetValue(pwa);
            bool hasPairOfIrons = (bool)_hasPairOfIronsTransformsField.GetValue(pwa);

            if (adjustCollimatorsToTrajectory || hasPairOfIrons)
            {
                bool isAiming = pwa.IsAiming;
                float currentLosDeltaAngle = (float)_currentLosDeltaAngleField.GetValue(pwa);
                float losDeltaAngle = (float)_losDeltaAngleField.GetValue(pwa);

                float newCurrentLosDeltaAngle = Mathf.Lerp(currentLosDeltaAngle, isAiming ? losDeltaAngle : 0f, dt * 5f);
                _currentLosDeltaAngleField.SetValue(pwa, newCurrentLosDeltaAngle);

                if (Mathf.Abs((float)_currentLosDeltaAngleField.GetValue(pwa)) > 0.002f)
                {
                    float angle = (float)_currentLosDeltaAngleField.GetValue(pwa);
                    DeferredRotate(pwa, pwa.HandsContainer.Fireport.position, _tempRot * new Vector3(angle, 0f, 0f));
                }
            }
        }
    }
}

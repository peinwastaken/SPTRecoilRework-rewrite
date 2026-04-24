using BepInEx.Configuration;
using EFT;
using EFT.Animations;
using EFT.InventoryLogic;
using HarmonyLib;
using RecoilReworkClient.Config.Settings;
using RecoilReworkClient.Enum;
using RecoilReworkClient.Helpers;
using RecoilReworkClient.Models;
using System;
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
        
        public Player Player;
        public Weapon CurrentWeapon;
        public CaliberData CurrentCaliberData;
        public ProceduralWeaponAnimation ProceduralWeaponAnimation;
        public LeftStanceStateController LeftStanceController;
        
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

        public float RecoilAngleSpeedWhileShooting = 0.5f;
        public float RecoilAngleSpeedWhileIdle = 3f;
        public float PenaltyRecoveryDelayAfterShot = 0.2f;
        
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
        private float _timeSinceLastShot = 0f;

        private FieldInfo _adjustCollimatorsToTrajectoryField;
        private FieldInfo _hasPairOfIronsTransformsField;
        private FieldInfo _currentLosDeltaAngleField;
        private FieldInfo _losDeltaAngleField;
        private FieldInfo _targetScopeRotationField;
        private FieldInfo _displacementStrField;
        
        public bool IsPistol = false;
        public bool HasStock = false;
        public bool IsBullpup = false;

        private Texture2D _debugOverlayBg;

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
            _displacementStrField = AccessTools.Field(typeof(ProceduralWeaponAnimation), "_displacementStr");

            Instance = this;
            Player = GetComponent<Player>();
            LeftStanceController = GetComponent<LeftStanceStateController>();
            ProceduralWeaponAnimation = Player.ProceduralWeaponAnimation;
            
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

            _debugOverlayBg = new Texture2D(1, 1);
            _debugOverlayBg.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.5f));
            _debugOverlayBg.Apply();
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            
            _timeSinceLastShot += dt;
            
            WeaponKickSpring.Update(dt);
            WeaponPositionSpring.Update(dt);
            WeaponAngleSpring.Update(dt);
            
            CameraPositionSpring.Update(dt);
            CameraAngleSpring.Update(dt);

            if (CurrentWeapon == null) return;
            bool isAutoFireOn = ProceduralWeaponAnimation.Shootingg.NewShotRecoil.AutoFireOn;
            bool isRecoilRecovering = IsRecoilRecovering();
            Weapon.EFireMode fireMode = CurrentWeapon.FireMode.FireMode;
            bool isWeaponBurstOrAuto = fireMode == Weapon.EFireMode.fullauto || fireMode == Weapon.EFireMode.burst;
            
            if (
                !AngleSprayPenalty.ApproxEquals(0f) &&
                isRecoilRecovering)
            {
                AngleSprayPenalty = Mathf.Lerp(AngleSprayPenalty, 0f, AngleRecoverySpeed * Time.fixedDeltaTime);
            }

            if (isWeaponBurstOrAuto && isAutoFireOn || !isWeaponBurstOrAuto && isRecoilRecovering)
            {
                WeaponAngleSpring.Speed = RecoilAngleSpeedWhileShooting;
            }
            else
            {
                WeaponAngleSpring.Speed = RecoilAngleSpeedWhileIdle;
            }

            /*
            if (!isWeaponBurstOrAuto)
            {
                WeaponAngleSpring.Speed = Mathf.Lerp(WeaponAngleSpring.Speed, RecoilAngleSpeedWhileShooting, 4f * dt);
            }
            else if (isRecoilRecovering)
            {
                WeaponAngleSpring.Speed = Mathf.Lerp(WeaponAngleSpring.Speed, RecoilAngleSpeedWhileIdle, 4f * dt);
            }
            else
            {
                WeaponAngleSpring.Speed = Mathf.Lerp(WeaponAngleSpring.Speed, RecoilAngleSpeedWhileShooting, 4f * dt);
            }*/
        }

        private bool IsRecoilRecovering()
        {
            return _timeSinceLastShot > PenaltyRecoveryDelayAfterShot;
        }

        public void RecalculateRecoilForces(ProceduralWeaponAnimation pwa, Weapon weapon)
        {
            CurrentWeapon = weapon;
            
            CurrentCaliberData = weapon.GetCaliberData();
            RecoilModifierData recoilData = weapon.GetModifierData();
            float totalWeight = weapon.TotalWeight;
            
            IsPistol = weapon.IsPistol();
            HasStock = weapon.HasStock();
            IsBullpup = weapon.IsBullpup();
            
            float verticalKickModifier = recoilData.VerticalKickMultiplier;
            float horizontalKickModifier = recoilData.HorizontalKickMultiplier;
            
            float verticalKick = CurrentCaliberData.BaseVerticalKick * verticalKickModifier * BaseRecoilSettings.BaseKickMultiplier.Value.x;
            float horizontalKick = CurrentCaliberData.BaseHorizontalKick * horizontalKickModifier * BaseRecoilSettings.BaseKickMultiplier.Value.y;
            float rollKick = CurrentCaliberData.BaseRollKick;

            float verticalAngle = verticalKick * BaseRecoilSettings.WeaponKickToAngleMult.Value * BaseRecoilSettings.BaseAngleMultiplier.Value.x;
            float horizontalAngle = horizontalKick * BaseRecoilSettings.WeaponKickToAngleMult.Value * BaseRecoilSettings.BaseAngleMultiplier.Value.y;
            
            float caliberEnergy = new Vector2(CurrentCaliberData.BaseVerticalKick, CurrentCaliberData.BaseHorizontalKick).magnitude * 0.01f;
            
            float backwardsVertMult = 1f + BaseRecoilSettings.BackwardsToAngleRecoilModifier.Value.x * Mathf.Pow(PositionBackwardsForce, 2f);
            float backwardsHorMult = 1f + BaseRecoilSettings.BackwardsToAngleRecoilModifier.Value.y * Mathf.Pow(PositionBackwardsForce, 2f);
            verticalAngle *= backwardsVertMult;
            horizontalAngle *= backwardsHorMult;

            float backwardsRecoilClamped = Mathf.Clamp(CurrentCaliberData.BaseBackwardsRecoil, 0f, 4f);
            
            // set caliber kick and recoil properties
            KickPitchForce = verticalKick;
            KickYawForce = horizontalKick;
            KickRollForce = rollKick;

            // TODO: figure out a better way to handle recoildelta for attachments
            // probably use just foregrips, stocks and muzzle brakes to calculate recoil delta
            /*
            AnglePitchForce = verticalAngle + verticalAngle * weapon.RecoilDelta;
            AngleYawForce = horizontalAngle + horizontalAngle * weapon.RecoilDelta;
            */

            float recoilReduction = weapon.GetRecoilReduction();
            AnglePitchForce = verticalAngle * Mathf.Clamp01(1f + recoilReduction);
            AngleYawForce = horizontalAngle * Mathf.Clamp01(1f + recoilReduction);
            
            PositionBackwardsForce = backwardsRecoilClamped;

            // calculate weapon kick damping
            float baseKickDamping = 0.3f;
            float kickWeightCoefficient = 0.02f;
            float finalKickDamping = baseKickDamping + (totalWeight * kickWeightCoefficient);
            WeaponKickSpring.DampingRatio = finalKickDamping;
            
            // calculate position spring damping and frequency from caliber backwards force
            float baseBackwardsFrequency = 5f;
            float finalPositionFrequency = baseBackwardsFrequency - (Mathf.Pow(CurrentCaliberData.BaseBackwardsRecoil, 2) * 0.025f);
            float baseBackwardsDamping = 1f;
            float finalBackwardsDamping = baseBackwardsDamping + (Mathf.Pow(CurrentCaliberData.BaseBackwardsRecoil, 2) * 0.01f);
            WeaponPositionSpring.Frequency = Mathf.Max(finalPositionFrequency, 1f);
            WeaponPositionSpring.DampingRatio = finalBackwardsDamping;
            
            // calculate weapon spray penalty recovery speed
            float angleRecoverySpeed = Mathf.Max(8f - Mathf.Exp(SprayPenaltySettings.WeightToPenaltyRecoveryModifier.Value * weapon.TotalWeight), 1f);
            AngleRecoverySpeed = angleRecoverySpeed;
            
            // calculate camera pitch/yaw visual recoil based on weapon backwards recoil amt
            camAng.x = PositionBackwardsForce * 4f;
            camAng.y = PositionBackwardsForce * 4f;

            if (IsBullpup || HasStock)
            {
                PenaltyRecoveryDelayAfterShot = 0.2f;
            }
            else
            {
                PenaltyRecoveryDelayAfterShot = 0.7f;
            }
            
            // recoil stuff based on if weapon is pistol or not
            if (IsPistol)
            {
                recoilBackPosCamVertOffsetMult = 0f;
                PositionUpwardsForce = 0f;
                pwa.CameraSmoothRecoil = GeneralSettings.PistolCameraSnap.Value;
            }
            else
            {
                recoilBackPosCamVertOffsetMult = -weapon.StockRecoilDelta;
                PositionUpwardsForce = PositionBackwardsForce * 0.07f;
                float cameraSnap = HasStock ? GeneralSettings.RifleCameraSnap.Value : GeneralSettings.RifleCameraSnapNoStock.Value;
                pwa.CameraSmoothRecoil = cameraSnap;

                if (!HasStock)
                {
                    PositionUpwardsForce += caliberEnergy * 0.1f;
                }
            }

            LeftStanceController.CalculateShoulderSwapSpeed(totalWeight);
            pwa.CrankRecoil = GeneralSettings.CrankRecoil.Value;
        }
        
        // TODO: condense this with helper functions? ffs
        public void OnShoot()
        {
            _timeSinceLastShot = 0f;
            bool hasStock = HasStock || IsBullpup;
            
            ProceduralWeaponAnimation pwa = Player.ProceduralWeaponAnimation;
            float stancePenaltyMult = PlayerHelper.GetStanceMultiplier(Player.Pose);
            float adsPenaltyMult = pwa.IsAiming ? StanceSettings.AimingPenaltyMult.Value : StanceSettings.HipfirePenaltyMult.Value;
            float mountPenaltyMult = pwa.IsMountedState || pwa.IsVerticalMounting ? StanceSettings.MountPenaltyMult.Value : 1f;
            
            float ammoRecoilModifier = (CurrentWeapon.CurrentAmmoTemplate?.ammoRec ?? 0f) * OnShotSettings.AmmoModifierMult.Value;

            float modifiedRecoilPitch = Mathf.Max(0f, AnglePitchForce + ammoRecoilModifier);
            float modifiedRecoilYaw = Mathf.Max(0f, AngleYawForce + ammoRecoilModifier);
            float backwardsRecoilCamAngMult = 1f + 0.05f * Mathf.Pow(PositionBackwardsForce, 2);

            float kickRoll = KickRollForce * Random.Range(0.7f, 1f);
            
            Vector3 recoilKickForce = new Vector3(-KickPitchForce, kickRoll, KickYawForce);
            Vector3 recoilAngForce = new Vector3(-modifiedRecoilPitch, AngleRollForce, modifiedRecoilYaw);
            Vector3 recoilPosForce = new Vector3(PositionSidewaysForce, PositionBackwardsForce, -PositionUpwardsForce);
            
            Vector3 camAngForce = new Vector3(Random.Range(-camAng.x, camAng.x), Random.Range(-camAng.y, camAng.y), camAng.z) * backwardsRecoilCamAngMult;
            
            recoilKickForce.z *= Random.Range(-1f, 1f);
            recoilAngForce.z *= Random.Range(-1f, 1f);
            recoilPosForce.x *= Random.Range(-1f, 1f);
            
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
            else
            {
                recoilAngForce.x *= 1.3f;
                recoilAngForce.y *= 1.3f;
                recoilAngForce.z *= 1.3f;
            }

            if (IsPistol)
            {
                recoilKickForce.x *= 3f;
                recoilKickForce.z *= 3f;
            }
            
            recoilKickForce *= Random.Range(0.9f, 1.1f);

            Vector2 cfgPitchPenalty = SprayPenaltySettings.PitchSprayPenaltyMult.Value;
            Vector2 cfgYawPenalty = SprayPenaltySettings.YawSprayPenaltyMult.Value;

            float pitchPenalty = AngleSprayPenalty * Random.Range(cfgPitchPenalty.x, cfgPitchPenalty.y);
            float yawPenalty = AngleSprayPenalty * Random.Range(cfgYawPenalty.x, cfgYawPenalty.y);
            
            recoilAngForce.x += recoilAngForce.x * Random.Range(0f, pitchPenalty);
            recoilAngForce.z += recoilAngForce.z * Random.Range(-yawPenalty, yawPenalty);

            recoilKickForce = Vector3.Scale(recoilKickForce, OnShotSettings.FinalWeaponKickMult.Value);
            recoilKickForce.y *= Random.Range(0.5f, 1.5f);

            Vector3 fireportDir = Player.ProceduralWeaponAnimation.HandsContainer.Fireport.transform.forward;
            Vector3 weaponPosForceDir = Vector3.Scale(fireportDir, recoilPosForce).normalized * recoilPosForce.y;
            
            WeaponKickSpring.ApplyImpulse(recoilKickForce);
            WeaponPositionSpring.ApplyImpulse(weaponPosForceDir);
            WeaponAngleSpring.ApplyImpulse(recoilAngForce);
            
            CameraPositionSpring.ApplyImpulse(camPos);
            CameraAngleSpring.ApplyImpulse(camAngForce);

            if (SprayPenaltySettings.EnableSprayPenalty.Value)
            {
                float caliberEnergy = new Vector2(CurrentCaliberData.BaseVerticalKick, CurrentCaliberData.BaseHorizontalKick).magnitude * 0.01f * SprayPenaltySettings.CaliberEnergyToPenaltyModifier.Value;
                AngleSprayPenalty += (1f - Mathf.Exp(-CurrentWeapon.TotalWeight * SprayPenaltySettings.WeightToPenaltyModifier.Value)) * caliberEnergy * stancePenaltyMult * adsPenaltyMult * mountPenaltyMult;
                AngleSprayPenalty = Mathf.Clamp(AngleSprayPenalty, 0f, SprayPenaltySettings.MaxSprayPenaltyMult.Value);
            }
        }

        public void ApplyComplexRotation(ProceduralWeaponAnimation pwa)
        {
            float dt = Time.deltaTime;
            
            _tempPos = pwa.HandsContainer.WeaponRootAnim.position;
            _tempRot = pwa.HandsContainer.WeaponRootAnim.rotation;
            _localRot = Quaternion.identity;

            float displacementStr = (float)_displacementStrField.GetValue(pwa);
            
            Vector3 handsRot = pwa.HandsContainer.HandsRotation.Get();
            Vector3 handSway = pwa.HandsContainer.SwaySpring.Value;
            Vector3 pivot = pwa.HandsContainer.WeaponRootAnim.TransformPoint(pwa.HandsContainer.RotationCenter);

            handsRot += displacementStr * (pwa.IsAiming ? pwa.AimingDisplacementStr : 1f) * new Vector3(handSway.x, 0f, handSway.z);
            handsRot += handSway;
            
            DeferredRotateCustomOrder(pwa, pivot, handsRot);

            // do recoil kick
            Vector3 recoilPivot = _tempPos + _tempRot * pwa.GetRecoilPivot();
            DeferredRotateCustomOrder(pwa, recoilPivot, WeaponKickSpring.Position);
            
            // do recoil angle
            DeferredRotateCustomOrder(pwa, recoilPivot, WeaponAngleSpring.Position);
            
            // do recoil position
            Vector3 up = _tempRot * Vector3.up;
            Vector3 recoilOffset = _tempRot * WeaponPositionSpring.Position;

// Component moving toward camera
            float towardCamera = Vector3.Dot(recoilOffset, -up);

// Only compress when moving toward camera
            if (towardCamera > 0f)
            {
                // Soft compression curve
                float compressed = towardCamera / (1f + towardCamera * 0.35f);
                recoilOffset += up * (towardCamera - compressed);
            }

            _tempPos += recoilOffset;
            
            ApplyAimingAlignment(dt);
            
            // current scope rotation
            Quaternion targetScopeRotation = (Quaternion)_targetScopeRotationField.GetValue(pwa);
            _scopeRot = Quaternion.Lerp(_scopeRot, pwa.IsAiming ? targetScopeRotation : Quaternion.identity, pwa.CameraSmoothTime * pwa.AimingSpeed * dt);
            
            pwa.HandsContainer.WeaponRootAnim.SetPositionAndRotation(_tempPos, _tempRot * _scopeRot);
        }
        
        private void DeferredRotateCustomOrder(ProceduralWeaponAnimation pwa, Vector3 worldPivot, Vector3 rotation)
        {
            _localRot = Quaternion.Euler(rotation.x, 0f, rotation.z) * Quaternion.Euler(0f, rotation.y, 0f) * _localRot;

            Quaternion quat1 = pwa.HandsContainer.WeaponRootAnim.parent.rotation * _localRot;
            Quaternion quat2 = quat1 * Quaternion.Inverse(_tempRot);
            Vector3 vec1 = _tempPos - worldPivot;
            vec1 = quat2 * vec1;

            _tempRot = quat1;
            _tempPos = worldPivot + vec1;
        }

        private void DeferredRotate(ProceduralWeaponAnimation pwa, Vector3 worldPivot, Vector3 rotation)
        {
            Quaternion quaternion = Quaternion.Euler(rotation);
            Quaternion temporaryRotation = quaternion * _tempRot;
            Vector3 vector = _tempPos - worldPivot;
            vector = quaternion * vector;
            _tempRot = temporaryRotation;
            _tempPos = worldPivot + vector;
        }
        
        private void ApplyAimingAlignment(float dt)
        {
            ProceduralWeaponAnimation pwa = Player.ProceduralWeaponAnimation;
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
        
        private void OnGUI()
        {
            if (!DebugSettings.EnableDebugOverlay.Value) return;
            
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                normal = {
                    textColor = Color.white,
                    background = _debugOverlayBg
                },
                
            };

            GUIStyle header = new GUIStyle(GUI.skin.label)
            {
                fontSize = 20,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };

            GUIStyle divider = new GUIStyle(GUI.skin.box)
            {
                normal = { background = Texture2D.whiteTexture },
                margin = new RectOffset(0, 0, 1, 1),
                fixedHeight = 1,
            };
            
            GUILayout.BeginVertical(style);
            GUILayout.Label("Weapon parameters", header);
            GUILayout.Label($"Kick Pitch Force: {KickPitchForce:F2}");
            GUILayout.Label($"Kick Yaw Force: {KickYawForce:F2}");
            GUILayout.Label($"Kick Roll Force: {KickRollForce:F2}");
            GUILayout.Box(GUIContent.none, divider, GUILayout.ExpandWidth(true));
            GUILayout.Label($"Angle Pitch Force: {AnglePitchForce:F2}");
            GUILayout.Label($"Angle Pitch Force: {AngleYawForce:F2}");
            GUILayout.Label($"Angle Pitch Force: {AngleRollForce:F2}");
            GUILayout.Box(GUIContent.none, divider, GUILayout.ExpandWidth(true));
            GUILayout.Label($"Is Pistol: {IsPistol}");
            GUILayout.Label($"Has Stock: {HasStock}");
            GUILayout.Label($"Is Bullpup: {IsBullpup}");
            
            GUILayout.Label("Recoil parameters", header);
            GUILayout.Label($"Spray Penalty: {AngleSprayPenalty:F2}");
            GUILayout.Label($"Penalty Recovery Speed: {AngleRecoverySpeed:F2}");
            GUILayout.EndVertical();
            
            Rect rect = GUILayoutUtility.GetLastRect();
        }
    }
}

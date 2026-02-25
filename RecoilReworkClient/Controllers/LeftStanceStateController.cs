using EFT;
using EFT.Animations;
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace RecoilReworkClient.Controllers
{
    public class LeftStanceStateController : MonoBehaviour
    {
        public Player Player;
        
        public bool IsLeftStance = false;
        
        public Vector3 LeftStancePositionOffset = Vector3.zero;
        public Vector3 LeftStanceRotationOffset = Vector3.zero;
        
        public float LeftStancePositionSpeed = 11.5f;
        public float LeftStanceRotationSpeed = 10f;
        
        public Vector3 LeftStancePositionOffsetMult = new Vector3(-0.2f, -0.1f, 0.06f);
        public Vector3 LeftStanceRotationOffsetMult = new Vector3(10f, 1f, 7f);

        private float _leftStancePositionCurve = 0f;
        private float _leftStanceRotationCurve = 0f;
        private AnimationCurve _lsHorizontalPosCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        private AnimationCurve _lsVerticalPosCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(0.5f, 1f, 0f, 0f),
            new Keyframe(1f, 0f, 0f, 0f)
        );
        private AnimationCurve _lsForwardsPosAnimCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(0.5f, 1f, 0f, 0f),
            new Keyframe(1f, 0f, 0f, 0f)
        );
        private AnimationCurve _lsWeaponRotationCurve = new AnimationCurve(
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(0.5f, 1f, 0f, 0f),
            new Keyframe(1f, 0f, 0f, 0f)
        );
        
        private void Awake()
        {
            Player = GetComponent<Player>();
        }

        public void ToggleLeftStance()
        {
            SetLeftStance(!IsLeftStance);
        }

        public void SetLeftStance(bool newState)
        {
            IsLeftStance = newState;
        }

        private void InterpolateStance(float value, float dt)
        {
            _leftStancePositionCurve = Mathf.SmoothStep(_leftStancePositionCurve, value, dt * LeftStancePositionSpeed);
            _leftStanceRotationCurve = Mathf.SmoothStep(_leftStanceRotationCurve, value, dt * LeftStanceRotationSpeed);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            InterpolateStance(IsLeftStance ? 1f : 0f, dt);

            float posX = _lsHorizontalPosCurve.Evaluate(_leftStancePositionCurve);
            float posY = _lsForwardsPosAnimCurve.Evaluate(_leftStancePositionCurve);
            float posZ = _lsVerticalPosCurve.Evaluate(_leftStancePositionCurve);
            
            float angX = _lsWeaponRotationCurve.Evaluate(_leftStanceRotationCurve);
            float angY = -8f * _leftStanceRotationCurve;
            float angZ = _lsWeaponRotationCurve.Evaluate(_leftStanceRotationCurve);
            
            LeftStancePositionOffset = new Vector3(posX, posY, posZ);
            LeftStanceRotationOffset = new Vector3(angX, angY, angZ);
        }

        public void ApplyRotations(ProceduralWeaponAnimation pwa)
        {
            ApplyComplexRotation(pwa);
            ApplySimpleRotation(pwa);
        }
        
        public void ApplyComplexRotation(ProceduralWeaponAnimation pwa)
        {
            if (IsLeftStance)
            {
                Plugin.Logger.LogWarning($"IS LEFT STANCE: {Player.name}");
            }
            
            // TODO: asjidosajidsioa
            Vector3 posOffset = LeftStancePositionOffset;
            posOffset.Scale(LeftStancePositionOffsetMult);

            Vector3 angOffset = LeftStanceRotationOffset;
            angOffset.Scale(LeftStanceRotationOffsetMult);
            
            pwa.HandsContainer.WeaponRootAnim.localPosition += posOffset;
            pwa.HandsContainer.WeaponRootAnim.localEulerAngles += angOffset;
        }

        public void ApplySimpleRotation(ProceduralWeaponAnimation pwa)
        {
            
        }
    }
}

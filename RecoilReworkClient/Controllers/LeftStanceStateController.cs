using EFT;
using EFT.Animations;
using RecoilReworkClient.Animation;
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
        
        public float LeftStancePositionSpeed = 3f;
        public float LeftStanceRotationSpeed = 2.8f;
        
        public Vector3 LeftStancePositionOffsetMult = new Vector3(-0.2f, -0.1f, 0.06f);
        public Vector3 LeftStanceRotationOffsetMult = new Vector3(6f, -15f, -7f);
        
        private float _leftStancePositionCurve = 0f;
        private float _leftStanceRotationCurve = 0f;
        private AnimationCurve _lsHorizontalPosCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        private ReversibleCurve _lsVerticalPosCurve = new ReversibleCurve(
            1f,
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(0.6531179f, 1.002346f, 0f, 0f),
            new Keyframe(1f, 0f, 0f, 0f)
        );
        private ReversibleCurve _lsForwardsPosAnimCurve = new ReversibleCurve(
            1f,
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(0.1819199f, 0.4060913f, 4.197513f, 4.197513f),
            new Keyframe(0.5f, 1f, 0f, 0f),
            new Keyframe(0.7733642f, 0.05806886f, -4.211924f, -4.211924f),
            new Keyframe(1f, 0f, 0f, 0f)
        );
        private ReversibleCurve _lsWeaponRotationCurve = new ReversibleCurve(
            1f,
            new Keyframe(0f, 0f, 0f, 0f),
            new Keyframe(0.6531179f, 1.002346f, 0f, 0f),
            new Keyframe(1f, 0f, 0f, 0f)
        );
        private BlendValue _leftStanceYawBlend = new BlendValue(1f, -1f, 1f);
        private float _finalLeftStanceRoll = -8f;
        
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
            _leftStancePositionCurve = Mathf.Lerp(_leftStancePositionCurve, value, LeftStancePositionSpeed * dt);
            _leftStanceRotationCurve = Mathf.Lerp(_leftStanceRotationCurve, value, LeftStanceRotationSpeed * dt);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            float target = IsLeftStance ? 1f : 0f;

            InterpolateStance(target, dt);
            
            _lsForwardsPosAnimCurve.SetBlendTarget(target);
            _lsVerticalPosCurve.SetBlendTarget(target);
            _lsWeaponRotationCurve.SetBlendTarget(target);
            _leftStanceYawBlend.SetBlendTarget(target);
            
            _lsForwardsPosAnimCurve.Update(dt);
            _lsVerticalPosCurve.Update(dt);
            _lsWeaponRotationCurve.Update(dt);
            _leftStanceYawBlend.Update(dt);

            float posX = _lsHorizontalPosCurve.Evaluate(_leftStancePositionCurve);
            float posY = _lsForwardsPosAnimCurve.Evaluate(_leftStancePositionCurve);
            float posZ = _lsVerticalPosCurve.Evaluate(_leftStancePositionCurve);
            
            float angX = _lsWeaponRotationCurve.Evaluate(_leftStanceRotationCurve);
            float angY = _lsWeaponRotationCurve.Evaluate(_leftStanceRotationCurve);
            float angZ = _lsWeaponRotationCurve.Evaluate(_leftStanceRotationCurve) * _leftStanceYawBlend.Evaluate(_leftStanceRotationCurve);
            
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
            Vector3 posOffset = LeftStancePositionOffset;
            posOffset.Scale(LeftStancePositionOffsetMult);

            Vector3 angOffset = LeftStanceRotationOffset;
            angOffset.Scale(LeftStanceRotationOffsetMult);
            angOffset.y += _finalLeftStanceRoll * _leftStanceRotationCurve;
            
            pwa.HandsContainer.WeaponRootAnim.localPosition += posOffset;
            pwa.HandsContainer.WeaponRootAnim.localEulerAngles += angOffset;
        }

        public void ApplySimpleRotation(ProceduralWeaponAnimation pwa)
        {
            
        }
    }
}

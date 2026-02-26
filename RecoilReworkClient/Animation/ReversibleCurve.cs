using UnityEngine;

namespace RecoilReworkClient.Animation
{
    public class ReversibleCurve
    {
        public AnimationCurve AnimationCurve;
        
        private float _blendAmount;
        private readonly float _blendSpeed;
        private float _blendTarget;

        public ReversibleCurve(float blendSpeed, params Keyframe[] keyframes)
        {
            AnimationCurve = new AnimationCurve(keyframes);
            _blendSpeed = blendSpeed;
        }

        public void SetBlendTarget(float target)
        {
            _blendTarget = target;
        }

        public float Evaluate(float t)
        {
            float normalValue = AnimationCurve.Evaluate(t);
            float reversedValue = AnimationCurve.Evaluate(1f - t);
            float value = Mathf.Lerp(normalValue, reversedValue, _blendAmount);
            return value;
        }

        public void Update(float dt)
        {
            float delta = _blendSpeed * dt;
            _blendAmount = Mathf.MoveTowards(_blendAmount, _blendTarget, delta);
        }
    }
}

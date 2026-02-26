using UnityEngine;

namespace RecoilReworkClient.Animation
{
    public class BlendValue
    {
        private float _minValue;
        private float _maxValue;
        
        private float _blendAmount;
        private readonly float _blendSpeed;
        private float _blendTarget;

        public BlendValue(float blendSpeed, float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _blendSpeed = blendSpeed;
        }

        public void SetBlendTarget(float target)
        {
            _blendTarget = target;
        }

        public float Evaluate(float t)
        {
            float normalValue = Mathf.Lerp(_minValue, _maxValue, t);
            float reversedValue = Mathf.Lerp(_maxValue, _minValue, t);
            float value = Mathf.Lerp(_minValue, _maxValue, _blendAmount);
            return value;
        }

        public void Update(float dt)
        {
            float delta = _blendSpeed * dt;
            _blendAmount = Mathf.MoveTowards(_blendAmount, _blendTarget, delta);
        }
    }
}

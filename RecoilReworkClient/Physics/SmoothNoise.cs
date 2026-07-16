using UnityEngine;
using System;

namespace RecoilReworkClient.Physics
{
    public class SmoothNoise
    {
        public Vector2 Position = Vector2.zero;

        public float Intensity = 1f;
        public float Speed = 1f;

        private Vector2 _sampleOffset;
        private float _elapsedTime;
        private int _seed;

        public SmoothNoise()
        {
            _seed = (int)Time.realtimeSinceStartup;
        }

        public void Reset(Vector2 position = default)
        {
            _elapsedTime = 0f;
            Position = Vector2.zero;
        }

        public Vector2 Update(float deltaTime)
        {
            if (deltaTime <= 0f)
            {
                return Position;
            }

            _elapsedTime += deltaTime * Mathf.Max(0f, Speed);
            Position = Sample(_elapsedTime);

            return Position;
        }

        private Vector2 Sample(float time)
        {
            float x = Mathf.PerlinNoise(_sampleOffset.x + time, _sampleOffset.y) * 2f - 1f;
            float y = Mathf.PerlinNoise(_sampleOffset.x, _sampleOffset.y + time) * 2f - 1f;

            return new Vector2(x, y) * Intensity;
        }
    }
}

using UnityEngine;

namespace RecoilReworkClient.Physics
{
    public class Spring
    {
        public Vector3 Position = Vector3.zero;
        public Vector3 Velocity = Vector3.zero;
        public Vector3 Target = Vector3.zero;
        
        public float Frequency = 5f;
        public float DampingRatio = 1f;
        public float Mass = 1f;
        public float Speed = 1f;

        public void Reset(Vector3 v)
        {
            Position = v;
            Velocity = Vector3.zero;
        }

        public void ApplyImpulse(Vector3 dir)
        {
            Velocity += dir;
        }

        public Vector3 Update(float deltaTime)
        {
            if (Mass <= 0f) return Position;

            float dt = Mathf.Min(deltaTime * Speed, 0.05f);
            if (dt <= 0f) return Position;

            float omega = 2f * Mathf.PI * Frequency / Mathf.Sqrt(Mathf.Max(Mass, 0.0001f));
            float zeta = Mathf.Max(0f, DampingRatio);

            float f = 1f + 2f * dt * zeta * omega;
            float oo = omega * omega;
            float hoo = dt * oo;
            float hhoo = dt * hoo;
            float detInv = 1f / (f + hhoo);

            Vector3 x = f * Position + dt * Velocity + hhoo * Target;
            Vector3 v = Velocity + hoo * (Target - Position);

            Position = x * detInv;
            Velocity = v * detInv;

            return Position;
        }
    }
}

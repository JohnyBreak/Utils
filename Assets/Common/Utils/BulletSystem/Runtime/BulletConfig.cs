using Common.Utils.BulletSystem.External;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime
{
    public struct BulletConfig
    {
        public BulletType Type;
        public readonly float MaxDistance;
        public BulletView View;
        public float Damage;
        public Vector3 Velocity;
    }
}
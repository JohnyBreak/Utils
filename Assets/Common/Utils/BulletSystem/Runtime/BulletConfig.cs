using Common.Utils.BulletSystem.External;

namespace Common.Utils.BulletSystem.Runtime
{
    public struct BulletConfig
    {
        public BulletType Type;
        public float MaxDistance;
        public float Speed;
        public BulletView View;
        public float Damage;

        public BulletConfig(BulletType type,
            float maxDistance,
            float speed,
            BulletView view,
            float damage)
        {
            Type = type;
            MaxDistance = maxDistance;
            Speed = speed;
            View = view;
            Damage = damage;
        }
    }
}
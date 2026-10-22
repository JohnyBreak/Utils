namespace Common.Utils.BulletSystem.Runtime
{
    public struct BulletConfig
    {
        public BulletType Type;
        public float MaxDistance;
        public float Speed;
        public float Damage;

        public BulletConfig(BulletType type,
            float maxDistance,
            float speed,
            float damage)
        {
            Type = type;
            MaxDistance = maxDistance;
            Speed = speed;
            Damage = damage;
        }
    }
}
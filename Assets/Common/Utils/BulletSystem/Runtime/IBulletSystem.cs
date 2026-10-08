using Common.Utils.WeaponSystem.Runtime;

namespace Common.Utils.BulletSystem.Runtime
{
    public interface IBulletSystem
    {
        void HandleShot(ShotInfo info);
        void Update(float deltaTime);
    }
}
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.BulletProvider
{
    public class BulletProvider : IBulletProvider
    {
        public IBullet Get(BulletConfig config, Vector3 shotPosition)
        {
            var bullet = new Bullet();
            bullet.Initialize(config, shotPosition);
            return bullet;
        }

        public void Release(IBullet bullet)
        {
            Object.Destroy(bullet.GetView().gameObject);
        }
    }
}
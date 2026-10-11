using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.BulletProvider
{
    public class BulletProvider : IBulletProvider
    {
        public IBullet Get(
            BulletConfig config, 
            Vector3 shotPosition, 
            Vector3 direction)
        {
            var bullet = new Bullet();
            var newBulletView = Object.Instantiate(config.View, shotPosition, Quaternion.identity);
            newBulletView.transform.forward = direction;
            
            bullet.Initialize(
                config, 
                shotPosition, 
                direction, 
                newBulletView);
            
            return bullet;
        }

        public void Release(IBullet bullet)
        {
            Object.Destroy(bullet.GetView().gameObject);
            bullet.Expire();
        }
    }
}
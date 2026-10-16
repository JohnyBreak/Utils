using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime
{
    public class BulletInfo
    {
        public Vector3 ShotPosition;
        public Vector3 Direction;
        public BulletConfig Config;
        public int BulletAmount;

        public static BulletInfo SimpleShot(
            Vector3 shotPosition,
            Vector3 direction,
            BulletConfig config)
        {
            return new BulletInfo()
            {
                ShotPosition = shotPosition, 
                Direction = direction,
                Config = config,
                BulletAmount = 1
            }; 
        }
        
        public static BulletInfo ManyBulletShot(
            Vector3 shotPosition,
            Vector3 direction,
            BulletConfig config,
            int bulletAmount)
        {
            return new BulletInfo()
            {
                ShotPosition = shotPosition,
                Direction = direction,
                Config = config,
                BulletAmount = bulletAmount
            }; 
        }
    }
}
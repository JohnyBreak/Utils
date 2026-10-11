using Common.Utils.BulletSystem.Runtime;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime
{
    public struct ShotInfo
    {
        public Vector3 ShotPosition;
        public Vector3 Direction;
        public BulletConfig Config;
        public int BulletAmount;

        public static ShotInfo SimpleShot(
            Vector3 shotPosition,
            Vector3 direction,
            BulletConfig config)
        {
            return new ShotInfo()
            {
                ShotPosition = shotPosition, 
                Direction = direction,
                Config = config,
                BulletAmount = 1
            }; 
        }
        
        public static ShotInfo ManyBulletShot(
            Vector3 shotPosition,
            Vector3 direction,
            BulletConfig config,
            int bulletAmount)
        {
            return new ShotInfo()
            {
                ShotPosition = shotPosition,
                Direction = direction,
                Config = config,
                BulletAmount = bulletAmount
            }; 
        }
    }
}
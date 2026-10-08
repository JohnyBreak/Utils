using Common.Utils.BulletSystem.Runtime;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Runtime
{
    public struct ShotInfo
    {
        public Vector3 ShotPosition;
        public BulletConfig Config;
        public int BulletAmount;

        public static ShotInfo SimpleShot(
            Vector3 shotPosition,
            BulletConfig config)
        {
            return new ShotInfo()
            {
                ShotPosition = shotPosition,
                Config = config,
                BulletAmount = 1
            }; 
        }
        
        public static ShotInfo ManyBulletShot(
            Vector3 shotPosition,
            BulletConfig config,
            int bulletAmount)
        {
            return new ShotInfo()
            {
                ShotPosition = shotPosition,
                Config = config,
                BulletAmount = bulletAmount
            }; 
        }
    }
}
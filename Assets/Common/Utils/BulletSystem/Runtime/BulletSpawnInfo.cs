using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime
{
    public class BulletSpawnInfo
    {
        public Vector3 SpawnPosition;
        public Vector3 Direction;
        public BulletConfig Config;

        public BulletSpawnInfo(
            Vector3 shotPosition,
            Vector3 direction,
            BulletConfig config)
        {
            SpawnPosition = shotPosition;
            Direction = direction;
            Config = config;
        }
    }
}
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime.BulletProvider
{
    public interface IBulletProvider
    {
        IBullet Get(BulletConfig config, Vector3 shotPosition);
        void Release(IBullet bullet);
    }
}
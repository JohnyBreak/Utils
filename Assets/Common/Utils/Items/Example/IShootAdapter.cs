using Common.Utils.BulletSystem.Runtime;
using UnityEngine;

namespace Common.Utils.Items.Example
{
    public interface IShootAdapter
    {
        void Shoot(BulletConfig def, Vector3 position, Vector3 Direction);
    }
}
using System;
using System.Collections.Generic;
using Common.Utils.BulletSystem.External;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common.Utils.BulletSystem.Runtime.BulletProvider
{
    public class BulletProvider : IBulletProvider
    {
        private readonly Dictionary<BulletType, BulletView> _viewPrefabsMap;

        public BulletProvider(Dictionary<BulletType, BulletView> viewPrefabsMap)
        {
            _viewPrefabsMap = viewPrefabsMap;
        }

        public IBullet Get(
            BulletConfig config, 
            Vector3 shotPosition, 
            Vector3 direction)
        {
            if (!_viewPrefabsMap.TryGetValue(config.Type, out var prefab))
            {
                throw new Exception($"No view for {config.Type} bullet type");
            }
            
            var bullet = new Bullet();
            var newBulletView = Object.Instantiate(prefab, shotPosition, Quaternion.identity);
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
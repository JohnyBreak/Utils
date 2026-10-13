using System;
using System.Collections.Generic;
using Common.Utils.BulletSystem.External;
using Common.Utils.BulletSystem.Runtime;
using Common.Utils.BulletSystem.Runtime.BulletProvider;
using Common.Utils.BulletSystem.Runtime.CollisionStrategies;
using Common.Utils.BulletSystem.Runtime.DamageStrategies;
using Common.Utils.BulletSystem.Runtime.MoveStrategies;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.ActionHandlers;
using Common.Utils.Items.Runtime.Actions;
using Common.Utils.Items.Runtime.Components;
using UnityEngine;

namespace Common.Utils.Items.Test
{
    public class ItemsTest : MonoBehaviour
    {
        [SerializeField] private GunView _gunView;
        [SerializeField] private BulletView _bulletView;
        
        private WeaponProxy _weaponProxy;
        private IBulletSystem _bulletSystem;
        private IBulletsBehaviour _bulletsBehaviour;
        
        private void Start()
        {
            var gunData = new GunData(
                10,
                _gunView,
                new BulletConfig(BulletType.Simple, 9, 3f, _bulletView, 15));
            
            IBulletProvider provider = new BulletProvider();
            
            _bulletsBehaviour = new BulletsBehaviour(
                new SimpleBulletMoveStrategy(), 
                new LineCastStrategy(),
                new SimpleDamageStrategy(),
                provider.Release);

            
            _bulletSystem = new BulletSystem.Runtime.BulletSystem(
                provider,
                _bulletsBehaviour);
            
            IShootAdapter shootAdapter = new ShootAdapter(
                _bulletSystem);
            
            List <IItemComponent> components = new()
            {
                new ShootItemComponent(gunData),
                new ReloadItemComponent(gunData)
            };
            
            Item pistol = new Item(components);

            ItemActionService actionService = new ItemActionService(
                new Dictionary<(Type, Type), IActionHandler>()
            {
                {(typeof(UseAction), typeof(ShootItemComponent)), new ShootHandler(shootAdapter)},
                {(typeof(ReloadAction), typeof(ReloadItemComponent)), new ReloadHandler()}
            });

            var equipment = new Equipment();
            equipment.Equip(pistol);

            _weaponProxy = new WeaponProxy(equipment, actionService);
        }

        private void Update()
        {
            _bulletSystem?.Update(Time.deltaTime);
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        private void Reload()
        {
            _weaponProxy.Reload();
        }

        private void Shoot()
        {
            _weaponProxy.Use();
        }
    }
}
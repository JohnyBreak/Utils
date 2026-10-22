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
using Common.Utils.WeaponSystem.Example;
using UnityEngine;

namespace Common.Utils.Items.Example
{
    public class ItemsTest : MonoBehaviour
    {
        [SerializeField] private WeaponView weaponView;
        [SerializeField] private BulletView _bulletView;
        
        private WeaponProxy _weaponProxy;
        private IBulletSystem _bulletSystem;
        private IBulletsBehaviour _bulletsBehaviour;
        
        private void Start()
        {
            var viewsMap = new Dictionary<BulletType, BulletView>()
            {
                { BulletType.Simple, _bulletView }
            };
            
            IBulletProvider provider = new BulletProvider(viewsMap);
            
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
                new AmmoComponent(),
                new ReloadComponent()
            };
            
            Item pistol = new Item(components);

            ItemActionService actionService = new ItemActionService(
                new Dictionary<(Type, Type), IActionHandler>()
            {
                {(typeof(UseAction), typeof(AmmoComponent)), new ShootHandler(shootAdapter)},
                {(typeof(ReloadAction), typeof(ReloadComponent)), new ReloadHandler()}
            });

            var equipment = new Equipment.Runtime.Equipment();
            equipment.EquipWeapon(pistol);

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
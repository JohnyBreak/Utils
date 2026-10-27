using System;
using System.Collections.Generic;
using Common.Utils.BulletSystem.External;
using Common.Utils.BulletSystem.Runtime;
using Common.Utils.BulletSystem.Runtime.BulletProvider;
using Common.Utils.BulletSystem.Runtime.CollisionStrategies;
using Common.Utils.BulletSystem.Runtime.DamageStrategies;
using Common.Utils.BulletSystem.Runtime.MoveStrategies;
using Common.Utils.Equipment.Runtime;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.Components;
using Common.Utils.WeaponSystem.Example.AttackControlModes;
using Common.Utils.WeaponSystem.Example.ReloadControlMode;
using Common.Utils.WeaponSystem.Example.Strategies;
using Common.Utils.WeaponSystem.Example.WeaponChange;
using Common.Utils.WeaponSystem.Runtime.AttackControlMode;
using Common.Utils.WeaponSystem.Runtime.AttackStrategies;
using Common.Utils.WeaponSystem.Runtime.Components;
using Common.Utils.WeaponSystem.Runtime.Helpers;
using Common.Utils.WeaponSystem.Runtime.MediatorAdapters;
using Common.Utils.WeaponSystem.Runtime.ReloadControlMode;
using Common.Utils.WeaponSystem.Runtime.ReloadStrategies;
using UnityEngine;

namespace Common.Utils.WeaponSystem.Example.Test
{
    public class WeaponTest : MonoBehaviour
    {
        [SerializeField] private InputAdapter _input;
        [SerializeField] private WeaponView _weaponView;
        [SerializeField] private BulletView _bulletView;
        [SerializeField] private Transform _cameraT;
        [SerializeField] private float _pistolFireRate = 0.2f;
        [SerializeField] private float _uziFireRate = 0.1f;
        
        private IBulletSystem _bulletSystem;
        private IBulletsBehaviour _bulletsBehaviour;
        
        private IInputAdapter _inputAdapter;
        private IWeaponEquipment _equipment = new Equipment.Runtime.Equipment();
        private WeaponChangeMediator _weaponChangeMediator;
        private IItem _pistol;
        private IItem _uzi;
        private IItem _shotgun;
        
        private void Start()
        {
            _inputAdapter = _input;
            
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
            
            IShootAdapter shootAdapter = new BulletShootAdapter(
                _bulletSystem);

            IAimDirection aimDirection = new AimDirection(_cameraT);
            IReloadStrategy reloadStrategy = new BulletReloadStrategy();
            IshootPositionProvider shootPositionProvider = new ShootPositionProvider(_weaponView);

            var fireRateValidator = new FireRateValidator();
            
            IAttackStrategy shootStrategy = new SingleBulletShootStrategy(
                shootAdapter,
                aimDirection,
                shootPositionProvider,
                fireRateValidator);
            
            IStoppableAttackStrategy autoStrategy = new AutoBulletShootStrategy(
                shootStrategy,
                _bulletView,
                fireRateValidator);
            
            IAttackStrategy shotgunStrategy = new ShotgunShootStrategy(
                shootAdapter,
                aimDirection,
                shootPositionProvider,
                fireRateValidator);
            
            BulletConfig config = new BulletConfig(BulletType.Simple, 100, 15, 15);
            BulletConfig ShotgunBarrelConfig = new BulletConfig(BulletType.Simple, 100, 15, 5);

            var attackersMap = new Dictionary<Type, IAttackControlMode>()
            {
                {typeof(SingleClickShootComponent), new ClickAttackControlMode(
                    _inputAdapter,
                    _equipment,
                    shootStrategy)},
                
                {typeof(HoldShootComponent), new HoldAttackControlMode(
                    _inputAdapter,
                    _equipment,
                    autoStrategy)},
                
                {typeof(ShotgunShootComponent), new ClickAttackControlMode(
                    _inputAdapter,
                    _equipment,
                    shotgunStrategy)}
            };

            var clickReloader = new ClickReloadControlMode(
                _inputAdapter,
                _equipment,
                reloadStrategy);
            
            var reloadersMap = new Dictionary<Type, IReloadControlMode>()
            {
                {typeof(SimpleReloadComponent), clickReloader},
            };
            
            var listeners = new List<IWeaponChangeListener>()
            {
                new CurrentAttackMode(attackersMap),
                new CurrentReloadMode(reloadersMap)
            };
            
            _weaponChangeMediator = new WeaponChangeMediator(_equipment, listeners);
            
            _pistol = new Item(
                new List<IItemComponent>()
                {
                    new GunComponent(),
                    new SingleClickShootComponent(_pistolFireRate),
                    new SimpleReloadComponent(),
                    new BulletMagazine(config, 10)
                });
            
            _uzi = new Item(
                new List<IItemComponent>()
                {
                    new GunComponent(),
                    new HoldShootComponent(_uziFireRate),
                    new SimpleReloadComponent(),
                    new BulletMagazine(config, 45)
                });
            
            _shotgun = new Item(
                new List<IItemComponent>()
                {
                    new GunComponent(),
                    new ShotgunShootComponent(15, _pistolFireRate, 15),
                    new SimpleReloadComponent(),
                    new BulletMagazine(ShotgunBarrelConfig, 4)
                });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                _equipment.EquipWeapon(_pistol);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _equipment.EquipWeapon(_uzi);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _equipment.EquipWeapon(_shotgun);
            }
            _bulletSystem.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _weaponChangeMediator.Dispose();
        }
    }
}
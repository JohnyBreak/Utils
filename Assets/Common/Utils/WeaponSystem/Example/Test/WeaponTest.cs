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
    // у перезарядки должно быть время
    // во время перезарядки нельзя стрелять
    // перезарядку магазина нельзя прервать
    // но дробовики и ружья с перезарядкой по патрону можно прервать,
    // обычно ЛКМ, но можно сделать повторным нажатием на кнопку перезарядки
    
    // нужны разные атакеры, на сингл клик и холд
    // туда же можно отправить гранаты, сперва прицелился, потом бросил
    // или зажал, высчиталась сила броска и бросок происходит на отпускании кнопки

    // что я хочу как юзер кода
    // мне надо создать пистолет
    // дробовик
    // пулемет
    // биту
    // снайперка (тут особое прицеливание)
    // плазма ган
    
    // хочу написать новый тип боезапаса, если нужен новый
    
    // стрельба
    // написать логику реагирования на инпут стрельбы
    // например клик, холд, холд-релиз
    
    // перезарядка
    // написать логику реагирования на инпут перезарядки
    
    // прицеливание
    // как целиться, просто от плеча или с зумом
    
    // доступно ли парное использование
    
    // логику хвата 1 или 2 руки
    
    // альтернативное действие, подствольник или типа того
    
    // смена режима стрельбы

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
            
            IAttackStrategy shootStrategy = new SingleBulletShootStrategy(
                shootAdapter,
                aimDirection,
                shootPositionProvider);
            
            BulletConfig config = new BulletConfig(BulletType.Simple, 100, 15, 15);
                
            var attackersMap = new Dictionary<Type, IAttackControlMode>()
            {
                {typeof(SingleClickShootComponent), new ClickAttackControlMode(
                    _inputAdapter,
                    _equipment,
                    shootStrategy)},
                {typeof(HoldShootComponent), new HoldAttackControlMode(
                    _inputAdapter,
                    _equipment,
                    shootStrategy,
                    _bulletView)}
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
                    new BulletMagazine(config, 10)
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
            
            _bulletSystem.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _weaponChangeMediator.Dispose();
        }
    }
}
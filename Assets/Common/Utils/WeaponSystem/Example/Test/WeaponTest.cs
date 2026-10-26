using System;
using System.Collections.Generic;
using Common.Utils.BulletSystem.External;
using Common.Utils.BulletSystem.Runtime;
using Common.Utils.BulletSystem.Runtime.BulletProvider;
using Common.Utils.BulletSystem.Runtime.CollisionStrategies;
using Common.Utils.BulletSystem.Runtime.DamageStrategies;
using Common.Utils.BulletSystem.Runtime.MoveStrategies;
using Common.Utils.Equipment.Runtime;
using Common.Utils.Items.Example;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.Components;
using Common.Utils.WeaponSystem.Example.AttackComponents;
using Common.Utils.WeaponSystem.Example.AttackerExample;
using Common.Utils.WeaponSystem.Example.ReloadExample;
using Common.Utils.WeaponSystem.Example.Test.Pistol;
using Common.Utils.WeaponSystem.Example.WeaponChange;
using Common.Utils.WeaponSystem.Runtime;
using Common.Utils.WeaponSystem.Runtime.Magazine;
using Common.Utils.WeaponSystem.Runtime.ReloadStrategies;
using Common.Utils.WeaponSystem.Runtime.ShootStrategies;
using UnityEngine;
using IShootAdapter = Common.Utils.WeaponSystem.Runtime.IShootAdapter;

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
        [SerializeField] private float _fireRate = 0.3f;
        
        private WeaponProxy _weaponProxy;
        private IBulletSystem _bulletSystem;
        private IBulletsBehaviour _bulletsBehaviour;
        
        private IInputAdapter _inputAdapter;
        private IWeaponEquipment _equipment = new Equipment.Runtime.Equipment();
        private WeaponChangeObserver _weaponChangeObserver;
        
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
            IReloadStrategy reload = new BulletReloadStrategy();

            BulletConfig config = new BulletConfig(BulletType.Simple, 100, 15, 15);
            BulletAmmo ammo = new BulletAmmo(config, 10);
            
            //IMagazine magazine = new BulletMagazine(
            //    new HashSet<BulletType>() { BulletType.Simple }, 
            //    ammo,
            //    10);
            
            //IWeapon pistol = new ModularWeapon(
            //    new SingleBulletShootStrategy(shootAdapter, aimDirection, _fireRate),
            //    reload,
            //    magazine,
            //    _weaponView);
                
            var attackersMap = new Dictionary<Type, IAttackMode>()
            {
                {typeof(SingleClickShootComponent), new ClickAttackMode(_inputAdapter, _equipment)},
                {typeof(RepeatableShootComponent), new RepeatableAttackMode(
                    _inputAdapter, 
                    _equipment,
                    0.1f,
                    _bulletView)}
            };

            var clickReloader = new ClickReloadMode(_inputAdapter, _equipment);
            
            var reloadersMap = new Dictionary<Type, IReloadMode>()
            {
                {typeof(SimpleReloadComponent), clickReloader},
                {typeof(SimpleReloadComponent), clickReloader},
            };
            
            var listeners = new List<IWeaponChangeListener>()
            {
                new CurrentAttackMode(attackersMap),
                new Reloader(reloadersMap)
            };
            
            _weaponChangeObserver = new WeaponChangeObserver(_equipment, listeners);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Item item = new Item(
                    new List<IItemComponent>()
                    {
                        //new SingleClickAttackComponent()
                    });
                
                _equipment.EquipWeapon(item);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Item item = new Item(
                    new List<IItemComponent>()
                    {
                       //new RepeatableAttackComponent()
                    });
                
                _equipment.EquipWeapon(item);
            }
            
            _bulletSystem.Update(Time.deltaTime);
        }

        private void OnDestroy()
        {
            _weaponChangeObserver.Dispose();
        }
    }
}
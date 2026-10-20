using System;
using System.Collections.Generic;
using Common.Utils.BulletSystem.Runtime.CollisionStrategies;
using Common.Utils.BulletSystem.Runtime.DamageStrategies;
using Common.Utils.BulletSystem.Runtime.MoveStrategies;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime
{
    public class BulletBehaviour : IBulletBehaviour
    {
        // в компонентных итемах в компоненте shootComponent использовать shootService,
        // в который передать сам итем, сервис сам возьмет что ему надо,
        // создаст shotInfo и передаст в bulletSystem
        
        
        // общая WeaponSystem вокруг BulletSystem

        // снаружи создать пулю и передать в BulletSystem
        // когда пуля уже не нужна ее надо вернуть
        // в BulletSystem передадим колбэк куда будем передавать пулю

        // IBullet и конкретные классы, которые содержат в себе ссылку на GO вьюшку
        // отдельные настройки типа урона(у каких-то пуль может не быть урона)
        // количества отскоков при рикошете, дальность полета и т.д.

        // енум типов пуль, по этому типу будут определяться стратегии

        // система, которая двигает пули, проверяет попала ли пуля во что-то,
        // делегирует логику в стратегии, возвращает пулю, если она expired

        // стратегия движения 
        // стратегия определения попадания
        // стратегия нанесения урона

        // декораторы с мапами стратегий

        // пулл для пуль в WeaponSystem
        // билдер для пуль в WeaponSystem

        private readonly List<IBullet> _activeBullets = new List<IBullet>();
        private readonly IBulletMoveStrategy _moveStrategy;
        private readonly IBulletCollisionStrategy _collisionStrategy;
        private readonly IBulletDamageDeliveryStrategy _damageStrategy;
        private readonly Action<IBullet> _releaseBullet;// вынести в стратегию ?

        public BulletBehaviour(
            IBulletMoveStrategy moveStrategy,
            IBulletCollisionStrategy collisionStrategy,
            IBulletDamageDeliveryStrategy damageStrategy,
            Action<IBullet> releaseBullet)
        {
            _moveStrategy = moveStrategy;
            _collisionStrategy = collisionStrategy;
            _damageStrategy = damageStrategy;
            _releaseBullet = releaseBullet;
        }

        public void RegisterBullet(IBullet bullet)
        {
            _activeBullets.Add(bullet);
        }

        public void Update(float deltaTime)
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                var bullet = _activeBullets[i];

                _moveStrategy.Move(bullet, deltaTime);

                RaycastHit[] hits = _collisionStrategy.CastCollision(bullet);

                _damageStrategy.DeliverDamage(hits, bullet);

                if (bullet.IsExpired())// вынести в стратегию ?
                {
                    _activeBullets.RemoveAt(i);
                    _releaseBullet?.Invoke(bullet);
                    continue;
                }

                bullet.UpdateViewPosition();
            }
        }
    }
}
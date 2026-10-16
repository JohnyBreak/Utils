using Common.Utils.BulletSystem.External;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime
{
    public class Bullet : IBullet
    {
        private BulletType _type;
        private float _maxDistance;
        private float _traveledDistance;
        private bool _needExpire;
        private BulletView _view;
        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _velocity;
        private float _damage;

        public void Initialize(
            BulletConfig config, 
            Vector3 shotPosition, 
            Vector3 direction,
            BulletView view)
        {
            _type = config.Type;
            _maxDistance = config.MaxDistance;
            _view = view;
            _velocity = direction * config.Speed;
            _damage = config.Damage;
            
            _previousPosition = shotPosition;
            _currentPosition = shotPosition;
            Reset();
        }

        private void Reset()
        {
            _traveledDistance = 0;
            _needExpire = false;
        }

        public BulletType GetBulletType()
        {
            return _type;
        }

        public BulletView GetView()
        {
            return _view;
        }

        public Vector3 GetPreviousPosition()
        {
            return _previousPosition;
        }

        public Vector3 GetCurrentPosition()
        {
            return _currentPosition;
        }

        public void SetCurrentPosition(Vector3 position)
        {
            _currentPosition = position;
        }

        public void UpdatePreviousPosition()
        {
            _previousPosition = _view.transform.position;
        }

        public Vector3 GetVelocity()
        {
            return _velocity;
        }

        public void UpdateViewPosition()
        {
            _view.transform.position = _currentPosition;
        }

        public void Expire()
        {
            _needExpire = true;
        }

        public bool IsExpired()
        {
            return _needExpire || _traveledDistance >= _maxDistance;
        }

        public void AddTraveledDistance(float displacement)
        {
            if (displacement <= 0)
            {
                return;
            }

            _traveledDistance += displacement;
        }

        public float GetDamage()
        {
            return _damage;
        }
    }
}
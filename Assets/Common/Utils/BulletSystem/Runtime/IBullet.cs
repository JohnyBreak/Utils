using Common.Utils.BulletSystem.External;
using Common.Utils.Damageable;
using UnityEngine;

namespace Common.Utils.BulletSystem.Runtime
{
    public interface IBullet // отрефачить, разделить на разные интерфейсы или объекты
    {
        // хочется отдельную сущность с типом оружия, куда подходит пуля, калибр(неизвестно зачем, видимо, по нему определять подходит ли пуля к оружию)
        // в эту сущность перенести тип пули, обычная, разрывная, скоростная и тд
        // что-то еще хотел в эту сущность добавить, забыл...
        BulletType GetBulletType();
        BulletView GetView();
        Vector3 GetPreviousPosition();
        Vector3 GetCurrentPosition();
        void UpdatePreviousPosition();
        Vector3 GetVelocity();
        void SetCurrentPosition(Vector3 position);
        void UpdateViewPosition();
        void Expire();
        bool IsExpired();
        void AddTraveledDistance(float displacement);
        Damage GetDamage();
    }
}
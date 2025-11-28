using System.Collections.Generic;
using Collectables.View;
using UnityEngine;

namespace Collectables.UI
{
    // сделать канвас для текста, пропихнуть во все коллектаблы метод вызова текста и скрытия текста
    // сделать один апдейт чтобы обновлять все тексты
    // нужно прокинуть текст и объект, а может и сам объект, просто сделать метод tostring, который будет возвращать текст название х кол-во
    // сделать словарь из объектов, ключ это хэшкод коллектабла, значение это объект текста
    
    //объект текста, в нем содержатся анимации и ссылка на текст
    
    // сделать пул текстов
    
    public class CollectableFloatingTextCanvas : MonoBehaviour
    {
        [SerializeField] private CollectableFloatingText _textPrefab;
        [SerializeField] private RectTransform _parent;
        
        private Dictionary<int, CollectableTextHolder> _textHoldersMap = new();
        private float _dorderSize = 100;
        
        public void Show(CollectableObjectView collectable)
        {
            if (_textHoldersMap.ContainsKey(collectable.HashCode))
            {
                return;
            }
            
            var text = Instantiate(_textPrefab, _parent);
            _textHoldersMap[collectable.HashCode] = new CollectableTextHolder(collectable, text);
        }

        public void Hide(CollectableObjectView collectable)
        {
            if (!_textHoldersMap.TryGetValue(collectable.HashCode, out var holder))
            {
                return;
            }
            
            Destroy(holder.Text.gameObject);
            _textHoldersMap.Remove(collectable.HashCode);
        }

        public void SetRedText(CollectableObjectView collectable)
        {
            if (!_textHoldersMap.ContainsKey(collectable.HashCode))
            {
                return;
            }
            
            _textHoldersMap[collectable.HashCode].Text.SetRedText();
        }

        private void LateUpdate()
        {
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            foreach (var holder in _textHoldersMap.Values)
            {
                holder.Text.UpdatePosition(CalculateScreenPosition(holder.View.transform.position));
            }
        }

        private Vector3 CalculateScreenPosition(Vector3 viewPosition)
        {
            var targetPosition = viewPosition + Vector3.up;
            var targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
            bool isOffscreen = targetPositionScreenPoint.x <= _dorderSize
                               || targetPositionScreenPoint.x >= Screen.width - _dorderSize
                               || targetPositionScreenPoint.y <= _dorderSize
                               || targetPositionScreenPoint.y >= Screen.height - _dorderSize;

            if (isOffscreen)
            {
                if (targetPositionScreenPoint.x <= _dorderSize)
                {
                    targetPositionScreenPoint.x = _dorderSize;
                }
                if (targetPositionScreenPoint.x >= Screen.width - _dorderSize)
                {
                    targetPositionScreenPoint.x = Screen.width - _dorderSize;
                }
                if (targetPositionScreenPoint.y <= _dorderSize)
                {
                    targetPositionScreenPoint.y = _dorderSize;
                }
                if (targetPositionScreenPoint.y >= Screen.height - _dorderSize)
                {
                    targetPositionScreenPoint.y = Screen.height - _dorderSize;
                }
            }


            return targetPositionScreenPoint;
        }
    }

    internal class CollectableTextHolder // чтобы брать позицию объекта при обновлении
    {
        public readonly CollectableObjectView View;
        public readonly CollectableFloatingText Text;

        public CollectableTextHolder(CollectableObjectView view, CollectableFloatingText text)
        {
            View = view;
            Text = text;
        }
    }
}
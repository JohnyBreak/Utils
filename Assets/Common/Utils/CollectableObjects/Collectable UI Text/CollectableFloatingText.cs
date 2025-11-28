using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Collectables.UI
{
    public class CollectableFloatingText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        private Sequence _sequence;

        public void Show(string text)
        {
            _text.text = text;
        }

        public void SetRedText()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.Append(_text.DOColor(Color.red, 0.2f));
            _sequence.AppendInterval(0.9f);
            _sequence.Append(_text.DOColor(Color.white, 0.75f));
        }

        public void UpdatePosition(Vector3 collectableScreenPosition)
        {
            transform.position = CorrectPosition(collectableScreenPosition);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
        }

        private Vector3 CorrectPosition(Vector3 collectableScreenPosition)
        {
            //проверить, если текст выодит за границы экрана и пододвинуть его, что бы был виден целиком
            return collectableScreenPosition;
        }
    }
}
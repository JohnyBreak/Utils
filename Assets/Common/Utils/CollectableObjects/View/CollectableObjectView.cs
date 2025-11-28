using System;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

namespace Collectables.View
{
    public class CollectableObjectView : MonoBehaviour, ICollectableView
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Collider _textCollider;
        [SerializeField] private LayerMask _mask;
        
        private Action<CollectableObjectView> _onEnter;
        private Action<CollectableObjectView> _onExit;
        private Action<CollectableObjectView, Action, Action> _onCollect;
        private CollectableConfig _config;
        private Vector3 _initialPosition;
        private Sequence _sequence;
        private CompositeDisposable _disposable = new CompositeDisposable();
        private int _hashCode = -1;
        
        public CollectableConfig Config => _config;
        
        public int HashCode {
            get
            {
                if (_hashCode < 0)
                {
                    _hashCode = this.GetHashCode();
                }

                return _hashCode;
            }
        }

        private void Start()
        {
            _textCollider
                .OnTriggerEnterAsObservable()
                .Subscribe(OnTextTriggerEnter)
                .AddTo(_disposable);
            _textCollider
                .OnTriggerExitAsObservable()
                .Subscribe(OnTextTriggerExit)
                .AddTo(_disposable);
            
            _initialPosition = transform.position;
            transform.DORotate(new Vector3(0, 360, 0), 2, RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetRelative()
                .SetEase(Ease.Linear);
        }

        private void OnTextTriggerEnter(Collider other)
        {
            if (((1 << other.gameObject.layer) & _mask) == 0)
            {
                return;
            }
            _onEnter?.Invoke(this);
        }
        
        private void OnTextTriggerExit(Collider other)
        {
            if (((1 << other.gameObject.layer) & _mask) == 0)
            {
                return;
            }
            _onExit?.Invoke(this);
        }
        
        public void Init(CollectableConfig collectable,
            Action<CollectableObjectView> onEnter,
            Action<CollectableObjectView> onExit,
            Action<CollectableObjectView, Action, Action> onCollect)
        {
            _config = collectable;
            _onEnter = onEnter;
            _onExit = onExit;
            _onCollect = onCollect;
        }

        public void Collect()
        {
            _onCollect?.Invoke(this, Success, Fail);
            return;
            
            void Success()
            {
                DOTween.Kill(this);
                Destroy(gameObject);
            }
            
            void Fail()
            {
                _collider.enabled = false;
                _sequence?.Kill();
                _sequence = DOTween.Sequence();
                _sequence.Append(transform.DOShakePosition(1, 0.3f, 5, 90f, false, true, ShakeRandomnessMode.Harmonic));
                _sequence.Append(transform.DOMove(_initialPosition, 0.1f));
                _sequence.OnComplete(() => _collider.enabled = true);
            }
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}


using UnityEngine.AddressableAssets;

namespace AssetProvider
{
    public class AssetHolder
    {
        private object _gameObject;
        private float _usedLastTime;
        private IKeyEvaluator _id;
        private bool _isLoaded;
        private bool _markToUnload;

		public object GameObject => _gameObject;
        public IKeyEvaluator Id => _id;
        public bool IsLoaded => _isLoaded;

        public bool MarkToUnload
        {
            get => _markToUnload;
            set => _markToUnload = value;
        }
        
        public float UsedLastTime
        {
            get => _usedLastTime;
            set => _usedLastTime = value;
        }

        public AssetHolder(object gameObject,
			float usedLastTime,
			IKeyEvaluator id,
			bool isLoaded)
		{
			_gameObject = gameObject;
            _usedLastTime = usedLastTime;
            _id = id;
            _isLoaded = isLoaded;
		}
    }
}
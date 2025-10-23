using UnityEngine.AddressableAssets;

namespace AssetProvider.KeyEvaluator
{
    public class StringKeyEvaluator : IKeyEvaluator
    {
        private readonly string _key;
        
        public object RuntimeKey => _key;
        
        public StringKeyEvaluator(string key)
        {
            _key = key;
        }

        public bool RuntimeKeyIsValid()
        {
            return !string.IsNullOrEmpty(_key);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as StringKeyEvaluator);
        }

        public bool Equals(StringKeyEvaluator other)
        {
            return other != null && _key == other._key;
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }
    }
}
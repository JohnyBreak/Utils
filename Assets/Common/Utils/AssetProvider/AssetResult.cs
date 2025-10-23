namespace AssetProvider
{
    public class AssetResult<T>
    {
        public readonly T Object;
        public readonly bool IsExist;

        public AssetResult(T result, bool isExist)
        {
            Object = result;
            IsExist = isExist;
        }
    }
}
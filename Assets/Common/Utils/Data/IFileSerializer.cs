namespace CubeTower.Common.Data
{
    public interface IFileSerializer
    {
        void Save<T>(T data, string path);
        
        T Load<T>(string path);
    }
}
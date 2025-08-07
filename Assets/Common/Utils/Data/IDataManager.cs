namespace CubeTower.Common.Data
{
    public interface IDataManager
    {
        IData GetData(string key);

        public void Save();
    }
}
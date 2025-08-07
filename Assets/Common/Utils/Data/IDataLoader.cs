namespace CubeTower.Common.Data
{
    public interface IDataLoader
    {
        bool Load(DataFull data, string path);
        bool Save(ObjectRepositoriesContainer container, string path);
    }
}
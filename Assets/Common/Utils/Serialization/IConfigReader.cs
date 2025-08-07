namespace Serialization
{
    public interface IConfigReader
    {
        T Read<T>(string json);
    }
}


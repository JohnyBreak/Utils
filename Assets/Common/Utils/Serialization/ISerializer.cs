using System;

namespace Serialization
{
    public interface ISerializer
    {
        T Read<T>(string data, Type type = null);

        string Write<T>(T data);
    }
}
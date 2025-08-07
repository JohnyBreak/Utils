namespace Serialization
{
    public class JsonNewtonsoftConfigReader : IConfigReader
    {
        private readonly ISerializer _serializer;

        public JsonNewtonsoftConfigReader(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public T Read<T>(string json)
        {
            return _serializer.Read<T>(json, typeof(T));
        }
    }
}
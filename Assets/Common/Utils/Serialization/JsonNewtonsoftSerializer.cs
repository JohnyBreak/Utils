using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Serialization
{
    public class JsonNewtonsoftSerializer : ISerializer
    {
        private JsonSerializerSettings _jsonSettings;

        public JsonNewtonsoftSerializer()
        {
            _jsonSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "d.M.yyyy HH:mm:ss",
                Formatting = Formatting.Indented
            };
        }

        public T Read<T>(string data, Type type = null)
        {
            try
            {
                var result = (T) JsonConvert.DeserializeObject(data, type, _jsonSettings);
                
                if (result == null)
                {
                    return default;
                }

                return result;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Serialization >>> " + e);
                return default;
            }
        }

        public string Write<T>(T data)
        {
            string result;

            try
            {
                result = JsonConvert.SerializeObject(data, _jsonSettings);
            }
            catch (Exception e)
            {
                Debug.LogError($"Serialization >>> {data.GetType()} has incorrect format");
                return string.Empty;
            }

            return result;
        }
    }
}
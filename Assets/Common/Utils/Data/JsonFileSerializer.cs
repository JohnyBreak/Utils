using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace CubeTower.Common.Data
{
    public class JsonFileSerializer : IFileSerializer
    {
        private readonly JsonSerializerSettings _settings; 
        public JsonFileSerializer()
        {
            _settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore,
                DateFormatString = "d.M.yyyy HH:mm:ss",
                Formatting = Formatting.Indented
            };
        }

        public void Save<T>(T data, string path)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, _settings); 
                File.WriteAllText(path, json);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError($"Serialization >>> {data.GetType()} has incorrect format");
            }
        }

        public T Load<T>(string path)
        {
            if (!File.Exists(path))
            {
                Debug.Log($"Serialization >>> File {path} not Existing in Load");
                return default;
            }
            try
            {
                var json = File.ReadAllText(path);
                var data = JsonConvert.DeserializeObject<T>(json, _settings);
                return data;
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
            }

            return default;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace CubeTower.Common.Data
{
    public class FileDataLoader : IDataLoader
    {
        private readonly IFileSerializer _fileSerializer;
        
        public FileDataLoader(IFileSerializer fileSerializer)
        {
            _fileSerializer = fileSerializer;
        }
        
        public bool Load(DataFull data, string path)
        {
            var result = _fileSerializer.Load<List<ObjectRepository>>(path);
            if (result != null)
            {
                List<ObjectRepository> newRepos = result;

                if (data == null)
                {
                    Debug.LogError("Data in load from file null what");
                    data = new DataFull();
                }
                
                foreach (var repo in newRepos)
                {
                    data.AddData(repo);
                }
            }
            else
            {
                Debug.Log("error in json deserializing. file: " + path);
            }
            
            return true;
        }

        public bool Save(ObjectRepositoriesContainer container, string path)
        {
            if (container != null && container.ObjectRepositories != null)
            {
                _fileSerializer.Save(container, path);
            }
            else
            {
                Debug.LogError("Saving >> Save cannot be done cause data not loaded");
                return false;
            }
            
            Debug.Log($"Saving >> {path} save successful");
            return true;
        }
    }
}
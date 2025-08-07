using System;
using System.Collections.Generic;
using System.IO;
using Serialization;
using UnityEngine;

namespace CubeTower.Common.Data
{
    public class DataManager : IDataManager
    {
        private const string SaveFileName ="Save.json";

        private readonly IDataLoader _dataLoader;
        private readonly Dictionary<string, IData> _dataNodes = new();
        private readonly Dictionary<string, Type> _dataNameToType = new();
        
        private readonly ISerializer _serializer;
        private readonly IFileSerializer _fileserializer;
        private readonly List<IData> _datas;
        private DataFull _dataFull;
        private string _folderPath;
        private string _filePath;
        
        public DataManager(
            IFileSerializer fileSerializer, 
            ISerializer serializer,
            List<IData> datas)
        {
            _folderPath = Path.Combine(Application.persistentDataPath, "Data");
            _filePath = $"{_folderPath}/{SaveFileName}";
            _fileserializer = fileSerializer;
            _serializer = serializer;
            _datas = datas;
            _dataLoader = new FileDataLoader(_fileserializer);
            
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
            
            foreach (var data in _datas)
            {
                _dataNameToType.Add(data.Name(), data.GetType());
            }
            
            Load();
            
            foreach (var data in _datas)
            {
                if (!_dataNodes.ContainsKey(data.Name()))
                {
                    _dataNodes.Add(data.Name(), data);
                }
                else if (_dataNodes[data.Name()] == null)
                {
                    _dataNodes[data.Name()] = data;
                }
            }
            
        }

        public IData GetData(string key)
        {
            if (!_dataNodes.ContainsKey(key))
            {
                return null;
            }

            return _dataNodes[key];

        }

        public void Save()
        {
            SaveInternal();
        }
        
        private void Load()
        {
            if (!File.Exists(_filePath))
            {
                Debug.Log($"DataManager >>> Data file {_filePath} does not exist");
                return;
            }

            var result = _fileserializer.Load<ObjectRepositoriesContainer>(_filePath);

            if (result == null)
            {
                Debug.LogError($"DataManager >>> file {_filePath} empty or something wrong when read");
                return;
            }
            
            SetSave(result);
        }
        
        private void SetSave(ObjectRepositoriesContainer objectRepositoriesContainer)
        {
            if (objectRepositoriesContainer?.ObjectRepositories == null)
            {
                return;
            }
            
            _dataNodes.Clear();
            
            foreach (var repository in objectRepositoriesContainer.ObjectRepositories)
            {
                if (repository.ObjectType == null || !_dataNameToType.ContainsKey(repository.ObjectType))
                {
                    continue; 
                }
            
                var dataResult = _serializer.Read<IData>(repository.Object, _dataNameToType[repository.ObjectType]);
                if (dataResult != null)
                {
                    _dataNodes.Add(repository.ObjectType, dataResult);
                }
                else
                {
                    Debug.LogError("DataManager >>> error with : " + repository.ObjectType);
                    return;
                }
            }
        }
        
        private void SaveInternal()
        {
            if (_dataNodes != null && _dataNodes.Count > 0)
            {
                var saveData = CreateSaveData();
                
                _dataLoader.Save(saveData, _filePath);

            }
            else
            {
                Debug.LogError("DataManager >> Save cannot be done cause data not loaded");
            }
        }
        
        private ObjectRepositoriesContainer CreateSaveData()
        {
            _dataFull = new DataFull();
            
            foreach (var dataNode in _dataNodes)
            {
                var save = _serializer.Write(dataNode.Value);
                
                _dataFull.AddData(new ObjectRepository(dataNode.Key, save));
            }

            return _dataFull.GetContainer();
        }
    }
}
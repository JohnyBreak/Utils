using System;
using Newtonsoft.Json;
using UnityEngine;

namespace CubeTower.Common.Data
{
    [Serializable, JsonObject(MemberSerialization.Fields)]
    public class ObjectRepository
    {
        [SerializeField] private string _objectType;
        [SerializeField] private string _object;

        public ObjectRepository()
        {

        }

        public ObjectRepository(string objectType, string obj)
        {
            _objectType = objectType;
            _object = obj;
        }

        public string ObjectType
        {
            get => _objectType;
            set => _objectType = value;
        }

        public string Object
        {
            get => _object;
            set => _object = value;
        }

        public override bool Equals(object obj)
        {
            if (obj is not ObjectRepository other)
            {
                return false;
            }

            return string.Equals(_objectType, other._objectType) && string.Equals(_object, other._object);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Object, ObjectType);
        }
    }
}
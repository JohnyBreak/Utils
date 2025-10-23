using UnityEngine;

namespace AssetProvider
{
    public struct TransformParams
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Transform Parent;

        public TransformParams(Vector3 position, Quaternion rotation, Transform parent)
        {
            Position = position;
            Rotation = rotation;
            Parent = parent;
        }

        public TransformParams(Vector3 position, Quaternion rotation) : this()
        {
            Position = position;
            Rotation = rotation;
        }

        public TransformParams(Vector3 position, Transform parent) : this()
        {
            Position = position;
            Parent = parent;
        }

        public TransformParams(Vector3 position) : this()
        {
            Position = position;
        }
    }
}
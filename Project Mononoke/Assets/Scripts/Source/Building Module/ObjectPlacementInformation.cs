using UnityEngine;

namespace Source.BuildingModule
{
    public readonly struct ObjectPlacementInformation<T> where T : Object
    {
        public T Prefab { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Transform Parent { get; }

        public ObjectPlacementInformation(T prefab, Vector3 worldPosition, Quaternion rotation, Transform parent)
        {
            Prefab = prefab;
            Position = worldPosition;
            Rotation = rotation;
            Parent = parent;
        }
    }
}

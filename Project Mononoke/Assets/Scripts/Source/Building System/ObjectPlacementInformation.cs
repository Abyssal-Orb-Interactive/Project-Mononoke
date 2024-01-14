using UnityEngine;

namespace Source.BuildingSystem
{
    public readonly struct ObjectPlacementInformation
    {
        public GameObject Prefab { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public Transform Parent { get; }

        public ObjectPlacementInformation(GameObject prefab, Vector3 worldPosition, Quaternion rotation, Transform parent)
        {
            Prefab = prefab;
            Position = worldPosition;
            Rotation = rotation;
            Parent = parent;
        }
    }
}

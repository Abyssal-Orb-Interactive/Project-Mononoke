using System;
using System.Collections.Generic;
using Source.BuildingModule.Buildings;
using UnityEngine;

namespace Source.BuildingModule
{
    [CreateAssetMenu]
    public class BuildingsDatabaseSo : ScriptableObject
    {
        [SerializeField] private List<BuildingData> buildingsData = null;
        public IReadOnlyList<BuildingData> BuildingsData => buildingsData;
    }

    [Serializable]
    public class BuildingData
    {
        [field: SerializeField] public string Name { get; private set; } = null;
        [field: SerializeField] public int ID { get; private set; } = -1;
        [field: SerializeField] public Vector2Int Sizes { get; private set; } = Vector2Int.zero;
        [field: SerializeField] public Building Prefab { get; private set; } = null;
    }
}
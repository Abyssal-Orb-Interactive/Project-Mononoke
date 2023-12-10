using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BuildingsDatabaseSO : ScriptableObject
{
    [SerializeField] private List<BuildingData> _buildingDatas;
    public IEnumerable<BuildingData> BuildingDatas => _buildingDatas;
}

[Serializable]
public class BuildingData
{
    [field: SerializeField] public string Name { get; private set; } = null;
    [field: SerializeField] public int ID { get; private set; } = -1;
    [field: SerializeField] public Vector2Int Sizes { get; private set; } = Vector2Int.zero;
    [field: SerializeField] public GameObject Prefab { get; private set; } = null;
}

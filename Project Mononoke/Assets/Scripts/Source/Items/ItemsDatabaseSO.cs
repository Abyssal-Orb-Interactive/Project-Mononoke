using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.ItemsModule
{
    [CreateAssetMenu]
    public class ItemsDatabaseSO : ScriptableObject
    {
        [SerializeField] private List<ItemData> _itemsData = null;

        public IReadOnlyList<ItemData> ItemsData => _itemsData;  

        [Serializable]
        public class ItemData 
        {
            [field: SerializeField] public string Name { get; private set; } = null;
            [field: SerializeField] public int ID { get; private set; } = -1;
            [field: SerializeField] public GameObject Prefab { get; private set; } = null;
            [field: SerializeField] public float Weight { get; private set; } = -1;
            [field: SerializeField] public float Volume { get; private set; } = -1;
            [field: SerializeField] public float Price { get; private set; } = -1;
            [field: SerializeField] public float Durability { get; private set; } = -1;
        }    
    }
}

using System;
using Base.Databases;
using UnityEngine;

namespace Scripts.Source.Craft_Module
{
    [Serializable]
    public class Ingredient
    {
        [field: SerializeField] public string ItemId { get; private set; }
        [field: SerializeField] private DatabaseSOBase Database{ get; set; }
        [field: SerializeField] public int Quantity { get; private set; }
        
        public DatabaseSO<T> GetDatabase<T>() where T : IDatabaseItem
        {
            return Database as DatabaseSO<T>;
        }
    }
}
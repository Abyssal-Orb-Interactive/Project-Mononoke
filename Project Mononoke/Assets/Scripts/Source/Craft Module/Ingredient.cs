using System;
using Source.ItemsModule;
using UnityEngine;

namespace Scripts.Source.Craft_Module
{
    [Serializable]
    public class Ingredient
    {
        [field: SerializeField] public string ItemId { get; private set; } = null;
        [field: SerializeField] public string ItemsDatabaseAddresablesKey { get; private set; } = null;
        [field: SerializeField] public int Quantity { get; private set; } = 0;

        public Ingredient(string itemId, string itemsDatabaseAddresablesKey, int quantity)
        {
            ItemId = itemId;
            ItemsDatabaseAddresablesKey = itemsDatabaseAddresablesKey;
            Quantity = quantity;
        }
    }
}
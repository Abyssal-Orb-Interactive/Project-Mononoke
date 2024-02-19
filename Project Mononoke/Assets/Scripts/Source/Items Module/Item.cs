using System;
using UnityEngine;

namespace Source.ItemsModule
{
    [Serializable]
    public class Item : IComparable<Item>
    {
        [field: SerializeField] public int ID { get; private set;}
        [field: SerializeReference] public ItemsDatabase<ItemData> Database { get; private set;}
        [field: SerializeField, Range(0.01f, 100f)] public float PercentsOfDurability { get; private set;}

        public Item(int iD, ItemsDatabase<ItemData> database)
        {
          ID = iD;
          Database = database;
        }
        public int CompareTo(Item other)
        {
            return PercentsOfDurability.CompareTo(other.PercentsOfDurability);
        }
    }
}

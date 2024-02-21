using System;
using UnityEngine;

namespace Source.ItemsModule
{
    [Serializable]
    public class Item<T> : IComparable<Item<T>> where T : ItemData
    {
        [field: SerializeField] public int ID { get; private set;}
        [field: SerializeReference] public ItemsDatabase<T> Database { get; private set;}
        [field: SerializeField, Range(0.01f, 100f)] public float PercentsOfDurability { get; private set;}

        public Item(int iD, ItemsDatabase<T> database)
        {
            ID = iD;
            Database = database;
        }
        public int CompareTo(Item<T> other)
        {
            return PercentsOfDurability.CompareTo(other.PercentsOfDurability);
        }
    }
}

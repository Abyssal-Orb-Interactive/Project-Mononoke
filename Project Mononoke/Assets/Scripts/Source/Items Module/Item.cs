using System;
using UnityEngine;

namespace Source.ItemsModule
{
    [Serializable]
    public class Item : IComparable<Item>
    {
        public UseBehaviour OnUse;

        [field: SerializeField] public string ID { get; private set;}
        [field: SerializeReference] public ItemsDatabase Database { get; private set;}
        [field: SerializeField, Range(0.01f, 100f)] public float PercentsOfDurability { get; private set;}

        public Item(string iD, ItemsDatabase database)
        {
            ID = iD;
            Database = database;
            Database.TryGetItemDataBy(ID, out var data);
            OnUse += data?.UseBehaviour;
        }
        
        public void UseMatterIn(object context)
        {
            if (OnUse == null)
            { 
                Database.TryGetItemDataBy(ID, out var data);
                OnUse += data.UseBehaviour;
            }
            OnUse?.Invoke(context);
        }
        
        public int CompareTo(Item other)
        {
            return PercentsOfDurability.CompareTo(other.PercentsOfDurability);
        }
    }
}

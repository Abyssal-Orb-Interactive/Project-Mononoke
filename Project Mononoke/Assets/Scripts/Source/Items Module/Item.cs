using System;
using UnityEngine;

namespace Source.ItemsModule
{
    [Serializable]
    public class Item : IComparable<Item>
    {
        public UseBehaviour OnUse;
        [field: SerializeField] public IItemData Data { get; private set;}
        [field: SerializeField, Range(0.01f, 100f)] public float PercentsOfDurability { get; private set;}

        public Item(IItemData data)
        {
            Data = data;
            OnUse += Data?.UseBehaviour;
        }
        
        public void UseMatterIn(object context)
        {
            OnUse ??= Data.UseBehaviour;
            OnUse?.Invoke(context);
        }
        
        public int CompareTo(Item other)
        {
            return PercentsOfDurability.CompareTo(other.PercentsOfDurability);
        }
    }
}

using System;
using Source.InventoryModule;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
    public class Manipulator
    {
        private readonly float _strength = 0f;
        private readonly float _capacity = 0f;
        public Item Item { get; private set; } = null;

        public event Action<Item> InManipulatorItemChanged; 

        public Manipulator(float strength, float capacity)
        {
            if (strength <= 0) throw new ArgumentException("Manipulator's strength must be bigger then 0");
            if (capacity <= 0) throw new ArgumentException("Manipulator's capacity must be bigger then 0");
            
            _strength = strength;
            _capacity = capacity;
        }

        public bool TryTake(Item item)
        {
            var data = item?.Data;
            if (data == null) return false;
            if(data.Weight > _strength || data.Volume > _capacity) return false;
            
            if (Item != null) return false;

            Take(item);
            return true;
        }

        private void Take(Item item)
        {
            Item = item;
            InManipulatorItemChanged?.Invoke(item);
        }

        public void UseTackedItemMatterIn(object context)
        {
            Item?.UseMatterIn(context);
        }

        public bool TryStashIn(Inventory inventory)
        {
            if (inventory == null || !HasItem()) return false;

            if (!inventory.TryAddItem(Item)) return false;
            
            Item = null;
            InManipulatorItemChanged?.Invoke(null);

            return true;
        }

        public bool HasItem()
        {
            return Item != null;
        }
    }
}

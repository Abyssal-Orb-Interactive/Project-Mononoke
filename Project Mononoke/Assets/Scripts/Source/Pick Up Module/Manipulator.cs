using System;
using Source.InventoryModule;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
    public class Manipulator
    {
        private readonly float _strength = 5f;
        private readonly float _volume = 2f;
        private Item<ItemData> _item = null;

        public event Action<Item<ItemData>> InManipulatorItemChanged; 

        public Manipulator(float strength, float volume)
        {
            if (strength < 0) _strength = 0;
            else if (volume < 0) _volume = 0;
            else
            {
                _strength = strength;
                _volume = volume;
            }
        }

        public bool TryTake(Item<ItemData> item)
        {
            if (!item.Database.TryGetItemDataBy(item.ID, out var data)) return false;
            if(data.Weight > _strength || data.Volume > _volume) return false;

            Take(item);
            return true;
        }

        private void Take(Item<ItemData> item)
        {
            _item = item;
            InManipulatorItemChanged?.Invoke(item);
        }

        public void UseTackedItemMatterIn(object context)
        {
            _item?.UseMatterIn(context);
        }

        public bool TryStashIn(Inventory inventory)
        {
            if (inventory == null) return false;
            
            var result = inventory.TryAddItem(_item);
            
            if (result) InManipulatorItemChanged?.Invoke(null);

            return result;
        }

        public bool HasItem()
        {
            return _item != null;
        }
    }
}

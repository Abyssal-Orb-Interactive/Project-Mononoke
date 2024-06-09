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
        private Item _item = null;

        public Item Item => _item;

        public event Action<Item> InManipulatorItemChanged; 

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

        public bool TryTake(Item item)
        {
            var data = item.Data;
            if (data == null) return false;
            if(data.Weight > _strength || data.Volume > _volume) return false;
            
            if (_item != null) return false;

            Take(item);
            return true;
        }

        private void Take(Item item)
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
            if (inventory == null || _item == null) return false;

            if (!inventory.TryAddItem(_item)) return false;
            
            _item = null;
            InManipulatorItemChanged?.Invoke(null);

            return true;
        }

        public bool HasItem()
        {
            return _item != null;
        }
    }
}

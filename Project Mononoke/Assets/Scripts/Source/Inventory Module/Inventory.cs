using System.Collections;
using System.Collections.Generic;
using Source.ItemsModule;

namespace Source.InventoryModule
{
    public class Inventory : IEnumerable<IPickUpable>
    {
        private float _weightCapacity = 0;
        private float _volumeCapacity = 0;
        private float _availableWeight = 0;
        private float _availableVolume = 0;

        public int ItemCapacity => _items.Capacity;

        private List<IPickUpable> _items = null;

      public Inventory(float weightCapacity, float volumeCapacity)
      {
        _weightCapacity = weightCapacity;
        _volumeCapacity = volumeCapacity;
        _availableWeight = _weightCapacity;
        _availableVolume = _volumeCapacity;
        _items = new(30);
      }  

      public bool TryAddItem(IPickUpable item)
      {
        if(item.Weight > _availableWeight || item.Volume > _availableVolume) return false;

        AddItem(item);
        return true;
      }

      private void AddItem(IPickUpable item)
      {
        _items.Add(item);
        _availableWeight -= item.Weight;
        _availableVolume -= item.Volume;
      }

      public void RemoveItem(IPickUpable item)
      {
        _items.Remove(item);
      }

        public IEnumerator<IPickUpable> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}

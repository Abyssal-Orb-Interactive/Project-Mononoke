using System;
using static Source.InventoryModule.Inventory;
using static Source.ItemsModule.TrashItemsDatabaseSO;

namespace Source.ItemsModule
{
    public class Item : IItem
    {
        public float Weight { get; } = 0f;

        public float Volume { get; } = 0f;

        public float Price { get; } = 0f;

        public float Durability { get; } = 0f;

        public int ID { get; } = 0;

        public IPickUpableDatabase Database { get; } = null;

        public Item(InventoryItem itemData)
        {
            if(!itemData.Database.TryGetItemDataBy(itemData.ID, out ItemData data)) return;
            Weight = data.Weight;
            Volume = data.Volume;
            Price = data.Price;
            Durability = data.Durability;
            ID = itemData.ID;
            Database = itemData.Database;
        }

        public void TakeDamage()
        {
            throw new NotImplementedException();
        }
    }
}

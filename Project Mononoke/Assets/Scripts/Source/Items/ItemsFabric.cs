using System;
using System.Linq;
using VContainer;

namespace Source.ItemsModule
{
    public static class ItemsFabric
    {
        private static ItemsDatabaseSO _itemsDatabase = null;
        [Inject] private static void Initialize(ItemsDatabaseSO itemsDatabase)
        {
            _itemsDatabase = itemsDatabase;
        }
        public static IItem CreateItemWith(int ID)
        {
            if (_itemsDatabase == null) throw new InvalidOperationException("ItemsDatabaseSO has not been initialized. Call Initialize method first.");

            ItemsDatabaseSO.ItemData itemData = _itemsDatabase.ItemsData.First(item => item.ID == ID) ?? throw new ArgumentException($"Item with ID {ID} not found in the database.");

            var item = new Item(itemData.Weight, itemData.Volume, itemData.Price, itemData.Durability);

            return item;
        }
    }
}

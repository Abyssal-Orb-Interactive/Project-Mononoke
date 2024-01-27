using System;
using System.Linq;
using VContainer;

namespace Source.ItemsModule
{
    public static class ItemsFabric
    {
        private static ItemsDatabaseSO _itemsDatabase = null;
        [Inject] public static void Initialize(ItemsDatabaseSO itemsDatabase)
        {
            _itemsDatabase = itemsDatabase;
        }
        public static IItem CreateItemWith(int ID)
        {
            if (_itemsDatabase == null) throw new InvalidOperationException("ItemsDatabaseSO has not been initialized. Call Initialize method first.");

            ItemsDatabaseSO.ItemData itemData = null;

            if (!_itemsDatabase.TryGetItemDataBy(ID, ref itemData)) return new Item(0, 0, 0, 0, null, "Missing Item");

            var item = new Item(itemData.Weight, itemData.Volume, itemData.Price, itemData.Durability, itemData.UIData.Icon, itemData.UIData.Description);

            return item;
        }
    }
}

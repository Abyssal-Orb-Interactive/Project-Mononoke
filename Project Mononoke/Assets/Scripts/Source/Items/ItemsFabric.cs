using System;
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


            if (!_itemsDatabase.TryGetItemDataBy(ID, out ItemsDatabaseSO.ItemData itemData)) return new Item(0, 0, 0, 0);

            var item = new Item(itemData.Weight, itemData.Volume, itemData.Price, itemData.Durability);

            return item;
        }
    }
}

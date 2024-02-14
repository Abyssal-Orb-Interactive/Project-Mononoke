using System;
using VContainer;
using static Source.InventoryModule.Inventory;

namespace Source.ItemsModule
{
    public static class ItemsFabric
    {
        private static TrashItemsDatabaseSO _itemsDatabase = null;
        [Inject] public static void Initialize(TrashItemsDatabaseSO itemsDatabase)
        {
            _itemsDatabase = itemsDatabase;
        }
        public static IItem CreateItemWith(int ID)
        {
            if (_itemsDatabase == null) throw new InvalidOperationException("ItemsDatabaseSO has not been initialized. Call Initialize method first.");


            if (!_itemsDatabase.TryGetItemDataBy(ID, out TrashItemsDatabaseSO.ItemData itemData)) return new Item(new InventoryItem(-1, null));

            var item = new Item(new InventoryItem(ID, _itemsDatabase));

            return item;
        }
    }
}

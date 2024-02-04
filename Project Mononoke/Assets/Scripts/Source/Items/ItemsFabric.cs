using System;
using VContainer;

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


            if (!_itemsDatabase.TryGetItemDataBy(ID, out TrashItemsDatabaseSO.ItemData itemData)) return new Item(0, 0, 0, 0);

            var item = new Item(itemData.Weight, itemData.Volume, itemData.Price, itemData.Durability);

            return item;
        }
    }
}

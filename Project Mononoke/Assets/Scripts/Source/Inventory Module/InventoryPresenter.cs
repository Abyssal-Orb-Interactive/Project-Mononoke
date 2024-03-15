using Source.InventoryModule.UI;
using Source.ItemsModule;
using UnityEngine;
using static Source.InventoryModule.ItemsStackFabric;

namespace Source.InventoryModule
{
    public class InventoryPresenter
    {
        private readonly Inventory _inventory = null;
        private readonly InventoryTableView _view = null;

        public InventoryPresenter(Inventory inventory, InventoryTableView view)
        {
            _inventory = inventory;
            _view = view;

            _inventory.ItemAdded += OnItemAdded;
            _inventory.ItemRemoved += OnItemRemoved;

            _view.ItemDropped += OnItemDropped;
            
            _view.InitializeInventoryPresenterWithCells(_inventory.Count * 2 + 10);
        }

        private void OnItemDropped(StackDataForUI stackData)
        {
            for (var i = 0; i < stackData.StackCount; i++)
            { 
                if(!_inventory.TryGetItem(stackData.ItemID, stackData.StackIndex, out var item)) return;
                ItemViewFabric.Create(item, Vector3.zero);
            }
        }

        private void OnItemRemoved(InventoryItemsStack stack, int stackIndex)
        {
            if(!stack.TryPeekItem(out var item)) return;
            _view.UpdateData(new StackDataForUI(item.ID, item.Database, stackIndex, stack.Count));
        }


        private void OnItemAdded(InventoryItemsStack stack, int stackIndex)
        {
            if(!stack.TryPeekItem(out var item)) return;
            _view.UpdateData(new StackDataForUI(item.ID, item.Database, stackIndex, stack.Count));
        }

        public class StackDataForUI
        {
            public string ItemID {get; private set;}

            public ItemsDatabase<ItemData> ItemDatabase {get; private set;}
            public int StackIndex {get; private set;}
            public int StackCount {get; private set;}

            public StackDataForUI(string itemID, ItemsDatabase<ItemData> itemDatabase, int stackIndex, int stackCount)
            {
                ItemID = itemID;
                ItemDatabase = itemDatabase;
                StackIndex = stackIndex;
                StackCount = stackCount;
            }
        }
    }
}

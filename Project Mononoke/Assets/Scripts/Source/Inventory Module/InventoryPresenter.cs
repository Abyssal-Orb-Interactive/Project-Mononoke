using Source.InventoryModule.UI;
using Source.ItemsModule;
using static Source.InventoryModule.Inventory;
using static Source.InventoryModule.InventoryItemsStackFabric;

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
            
            _view.InitializeInventoryPresenterWithCells(_inventory.Count * 2 + 10);
        }

        private void OnItemRemoved(InventoryItemsStack stack, int stackIndex)
        {
            if(!stack.TryPeekItem(out InventoryItem item)) return;
            _view.UpdateData(new StackDataForUI(item.ID, item.Database, stackIndex, stack.Count));
        }


        private void OnItemAdded(InventoryItemsStack stack, int stackIndex)
        {
            if(!stack.TryPeekItem(out InventoryItem item)) return;
            _view.UpdateData(new StackDataForUI(item.ID, item.Database, stackIndex, stack.Count));
        }

        public class StackDataForUI
        {
            public int ItemID {get; private set;}

            public PickUpableDatabase ItemDatabase {get; private set;}
            public int StackIndex {get; private set;}
            public int StackCount {get; private set;}

            public StackDataForUI(int itemID, PickUpableDatabase itemDatabase, int stackIndex, int stackCount)
            {
                ItemID = itemID;
                ItemDatabase = itemDatabase;
                StackIndex = stackIndex;
                StackCount = stackCount;
            }
        }
    }
}

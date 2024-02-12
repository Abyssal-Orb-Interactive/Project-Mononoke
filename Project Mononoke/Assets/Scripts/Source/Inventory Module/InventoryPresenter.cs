using Source.InventoryModule.UI;
using static Source.InventoryModule.Inventory;

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
            var cellIndex = 0;
            foreach(var stack in _inventory)
            {
                if(!stack.TryPeekItem(out InventoryItem item)) continue;
                _view.UpdateData(cellIndex, item);
                cellIndex++;
            }
        }

        private void OnItemRemoved(Inventory.InventoryItem item)
        {
            _view.UpdateData(item.ID, item);
        }


        private void OnItemAdded(Inventory.InventoryItem item)
        {
            _view.UpdateData(item.ID, item);
        }

    }
}

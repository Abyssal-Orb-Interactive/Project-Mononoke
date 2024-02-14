using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.Items;
using Source.ItemsModule;
using UnityEngine;

namespace Scripts.Source
{
    public class TestingInventory : MonoBehaviour
    {
        [SerializeField] private InventoryTableView _view = null;
        [SerializeField] private TrashItemsDatabaseSO _items = null;
        [SerializeField] private DroppedItem item = null;
        private readonly Inventory _inventory = new (weightCapacity: 100, volumeCapacity: 100);

        private void Start()
        {
            ItemsFabric.Initialize(_items);

            var inventoryPresenter = new InventoryPresenter(_inventory, _view);

            _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));
            _inventory.TryAddItem(ItemsFabric.CreateItemWith(1));
            _inventory.TryAddItem(ItemsFabric.CreateItemWith(2));
            _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));
            _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));
            _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));
            
            item.BeginPickUp();
        }
    }
}

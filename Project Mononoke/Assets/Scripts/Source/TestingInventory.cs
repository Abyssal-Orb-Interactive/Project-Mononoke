using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Scripts.Source
{
    public class TestingInventory : MonoBehaviour
    {
        [SerializeField] private InventoryTableView _view = null;
        [SerializeField] private TrashItemsDatabaseSO _items = null;
        [SerializeField] private PickUpper _pickUpper = null;

        private void Start()
        {
            ItemsFabric.Initialize(_items);

            var inventoryPresenter = new InventoryPresenter(_pickUpper.Inventory, _view);
        }
    }
}

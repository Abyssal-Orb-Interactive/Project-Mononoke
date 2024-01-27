using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using UnityEngine;

namespace Scripts.Source
{
    public class TestingInventory : MonoBehaviour
    {
        [SerializeField] private InventoryPresenter presenter = null;
        [SerializeField] private ItemsDatabaseSO items = null;
        private readonly Inventory _inventory = new (weightCapacity: 100, volumeCapacity: 100);

        void Start()
        {
            ItemsFabric.Initialize(items);

           _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));
           _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));
           _inventory.TryAddItem(ItemsFabric.CreateItemWith(0));

           presenter.Initialize(_inventory);
           presenter.PresentInventory();
        }
    }
}

using System;
using Source.BattleSystem;
using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    [RequireComponent(typeof(Damageable))]
    [RequireComponent(typeof(ItemLauncher))]
    public class Container : Building
    {
        protected Inventory _inventory = null;
        private Damageable _damageable = null;
        private InventoryMenu _inventoryMenu = null;
        private ItemLauncher _itemLauncher = null;

        public event Action ContainerOpened, ContainerClosed = null;

        public void Initialize(Inventory inventory, InventoryMenu inventoryMenu)
        {
            _inventory = inventory;
            _inventoryMenu = inventoryMenu;
            _damageable = GetComponent<Damageable>();
            _damageable.Death += OnContainerDestruction;
            _itemLauncher = GetComponent<ItemLauncher>();
            if(_inventoryMenu == null) return;
            _inventoryMenu.StatusChanged += OnMenuStatusChanging;
        }

        private void OnMenuStatusChanging(InventoryMenu menu, bool status)
        {
            if(status == false) ContainerClosed?.Invoke();
            else ContainerOpened?.Invoke();
        }

        private void OnContainerDestruction(IDamageable damageable)
        {
            var itemsPreparedToDrop = _inventory.GetAllItemsAndClearInventory();
            foreach (var item in itemsPreparedToDrop)
            {
                _itemLauncher.DropAndGetEndingDropPosition(item);
            }
        }

        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            _inventoryMenu.Toggle(true);
        }


        public void Add(Item item)
        {
            _inventory.TryAddItem(item);
        }
    }
}
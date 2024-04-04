using System;
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

        public Action<StackDataForUI> ItemEquipped = null;

        public InventoryPresenter(Inventory inventory, InventoryTableView view)
        {
            _inventory = inventory;
            _view = view;

            _inventory.ItemAdded += OnItemAdded;
            _inventory.ItemRemoved += OnItemRemoved;
            _inventory.ItemDropped += OnItemDropped;

            _view.ItemDropped += OnUIItemDropped;
            _view.ItemEquipped += OnItemEquipped;

            _view.InitializeInventoryPresenterWithCells(_inventory.Count * 2 + 10);
        }

        private void OnItemDropped(InventoryItemsStack stack, int stackIndex)
        {
            if(!stack.TryPeekItem(out var item)) return;
            _view.UpdateData(new StackDataForUI(item.Data, stackIndex, stack.Count));
        }

        private void OnItemEquipped(StackDataForUI item)
        {
            ItemEquipped?.Invoke(item);
        }

        private void OnUIItemDropped(StackDataForUI stackData)
        {
            for (var i = 0; i < stackData.StackCount; i++)
            {
                if(!_inventory.TryGetItem(stackData.ItemData.ID, stackData.StackIndex, out var item)) return;
                ItemViewFabric.Create(item, Vector3.zero);
            }
        }

        private void OnItemRemoved(InventoryItemsStack stack, int stackIndex, Item item)
        {
            _view.UpdateData(new StackDataForUI(item.Data, stackIndex, stack.Count));
        }


        private void OnItemAdded(InventoryItemsStack stack, int stackIndex)
        {
            if(!stack.TryPeekItem(out var item)) return;
            _view.UpdateData(new StackDataForUI(item.Data, stackIndex, stack.Count));
        }

        public class StackDataForUI
        {
            public IItemData ItemData {get; private set;}
            public int StackIndex {get; private set;}
            public int StackCount {get; private set;}

            public StackDataForUI(IItemData itemData, int stackIndex, int stackCount)
            {
                ItemData = itemData;
                StackIndex = stackIndex;
                StackCount = stackCount;
            }
        }
    }
}

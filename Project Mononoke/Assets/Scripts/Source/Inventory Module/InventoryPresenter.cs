using System;
using System.Collections.Generic;
using System.Linq;
using Base.Input;
using Source.BuildingModule;
using Source.BuildingModule.Buildings.UI;
using Source.Character.Movement;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using static Source.InventoryModule.ItemsStackFabric;

namespace Source.InventoryModule
{
    public class InventoryPresenter
    {
        private readonly Inventory _inventory = null;
        private readonly InventoryTableView _view = null;
        private readonly ItemChooseMenu _itemChooseMenu = null;
        private Building _interactionBuilding = null;

        private Transform _holdersTransform = null;

        public event Action<StackDataForUI> ItemEquipped = null;
        public event Action<Item, Building> ItemChosen = null; 

        public InventoryPresenter(Inventory inventory, InventoryTableView view, ItemChooseMenu chooseMenu, Transform holdersTransform)
        {
            _inventory = inventory;
            _view = view;
            _itemChooseMenu = chooseMenu;
            _holdersTransform = holdersTransform;

            _inventory.ItemAdded += OnItemAdded;
            _inventory.ItemRemoved += OnItemRemoved;
            _inventory.ItemDropped += OnItemDropped;

            _view.ItemDropped += OnUIItemDropped;
            _view.ItemEquipped += OnItemEquipped;

            _itemChooseMenu.ItemSelected += OnItemChooseMenuItemSelected;

            _view.InitializeInventoryPresenterWithCells(_inventory.Count * 2 + 10);
            _itemChooseMenu.InitializeInventoryPresenterWithCells(_inventory.Count * 2 + 10);
        }

        private void OnItemChooseMenuItemSelected(StackDataForUI stackData)
        {
            if(!_inventory.TryGetItem(stackData.ItemData.ID, stackData.StackIndex, out var item)) return;
            ItemChosen?.Invoke(item, _interactionBuilding);
            _itemChooseMenu.ToggleWith(false);
        }

        public void GetOnItemChoosingMenu(Building building)
        {
            _interactionBuilding = building;
            _itemChooseMenu.ToggleWith(true);
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
                var targetPosition = _holdersTransform.position +
                                     DirectionToVector3IsoConverter.ToVector(MovementDirection.East) * 0.5f;
                var itemView = ItemViewFabric.Create(item, _holdersTransform.position);
                itemView.GetComponent<ParabolicMotionAnimationPlayer>().PlayAnimationBetween(_holdersTransform.position, targetPosition);
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

        public void UpdateChooseMenuWith(InventoryFilters filter)
        {
            _itemChooseMenu.UpdateMenu(GetInventoryStacksWith(filter));
        }

        public List<StackDataForUI> GetInventoryStacksWith(InventoryFilters filter)
        {
            var register = _inventory.InventoryRegister;
            var resultData = new List<StackDataForUI>();
            foreach (var ID in register.Keys.Where(ID => register[ID].Count > 0))
            {
                foreach (var stack in register[ID])
                {
                    if(!stack.TryPeekItem(out var item)) continue;
                    if(item.Data is not SeedData) break;
                    resultData.Add(new StackDataForUI(item.Data, stack.StackIndex, stack.Count));
                }
            }

            return resultData;
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

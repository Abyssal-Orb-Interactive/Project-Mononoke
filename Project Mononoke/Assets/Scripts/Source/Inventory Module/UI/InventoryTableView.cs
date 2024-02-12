using System;
using System.Collections.Generic;
using Source.BuildingModule;
using Source.UI;
using UnityEngine;
using static Source.InventoryModule.Inventory;

namespace Source.InventoryModule.UI
{
    public class InventoryTableView : MonoBehaviour
    {
        [SerializeField] private ItemUIElement _itemUIElementPrefab = null;
        [SerializeField] private UIItemDescriptionWindow _descriptionWindow = null;
        [SerializeField] private RectTransform _itemUIElementsContainer = null;
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;
        [SerializeField] private MousePointerMover _mousePointer = null;

        private List<ItemUIElement> _inventoryPresenterCells = null;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDraggingRequested;
        public event Action<int,int> OnSwapItemsRequested;

        private int _currentDraggingItemIndex = -1;


        public void InitializeInventoryPresenterWithCells(int cellCount)
        {
            _inventoryPresenterCells ??= new();

            for (var i = 0; i < cellCount; i++)
            {
                AddInventoryCell();
            }
        }

        public void UpdateData(int itemIndex, InventoryItem item)
        {
            if(_inventoryPresenterCells.Count < itemIndex || itemIndex < 0) return;

            _inventoryPresenterCells[itemIndex].InitializeWith(item);
        }

        private void AddInventoryCell()
        {
            _inventoryPresenterCells ??= new();
            var itemUIElement = _objectPlacer.PlaceObject(new ObjectPlacementInformation<ItemUIElement>(_itemUIElementPrefab, Vector3.zero ,Quaternion.identity, _itemUIElementsContainer));
            itemUIElement.ResetData();
            itemUIElement.OnItemLeftClicked += HandleItemSelection;
            itemUIElement.OnItemBeginDrag += HandleBeginDrag;
            itemUIElement.OnItemDroppedOn += HandleSwap;
            itemUIElement.OnItemEndDrag += HandleEndDrag;
            itemUIElement.OnItemRightClicked += HandleShowItemActions;
            _inventoryPresenterCells.Add(itemUIElement);
        }

        private void ResetDraggingElement()
        {
            _mousePointer.Toggle(false);
            _currentDraggingItemIndex = -1;
        }

        private void HandleShowItemActions(ItemUIElement element)
        {
            throw new NotImplementedException();
        }

        private void HandleEndDrag(ItemUIElement element)
        {
            ResetSelection();
        }

        

        private void HandleSwap(ItemUIElement element)
        {
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;

            var itemBuffer = _inventoryPresenterCells[_currentDraggingItemIndex].ItemData;
            _inventoryPresenterCells[_currentDraggingItemIndex].InitializeWith(_inventoryPresenterCells[index].ItemData);
            _inventoryPresenterCells[index].InitializeWith(itemBuffer);
            OnSwapItemsRequested?.Invoke(_currentDraggingItemIndex, index);
            ResetDraggingElement();
        }

        private void HandleBeginDrag(ItemUIElement element)
        {
            ResetDraggingElement();
            if(EqualityComparer<InventoryItem>.Default.Equals(element.ItemData, default)) return;
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;
            _currentDraggingItemIndex = index;
            HandleItemSelection(element);
            OnStartDraggingRequested?.Invoke(_currentDraggingItemIndex);
            CreateDraggedItem(element.ItemData);
        }

        private void CreateDraggedItem(InventoryItem item)
        {
            _mousePointer.Toggle(true);
            _mousePointer.SetData(item);
        }

        private void HandleItemSelection(ItemUIElement element)
        {
            ResetSelection();
            if(EqualityComparer<InventoryItem>.Default.Equals(element.ItemData, default)) return;
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;
            OnDescriptionRequested?.Invoke(index);
            element.Select();
            _descriptionWindow.InitializeWith(element.ItemData);
        }

        private void ResetSelection()
        {
            _descriptionWindow.Reset();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach(var element in _inventoryPresenterCells)
            {
                element.Deselect();
            }
        }

        private void OnEnable()
        {
            _descriptionWindow.Reset();
        }
    }
}
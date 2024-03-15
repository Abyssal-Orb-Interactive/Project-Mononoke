using System;
using System.Collections.Generic;
using Source.BuildingModule;
using Source.ItemsModule;
using Source.UI;
using UnityEngine;
using static Source.InventoryModule.InventoryPresenter;

namespace Source.InventoryModule.UI
{
    public class InventoryTableView : MonoBehaviour
    {
        [SerializeField] private ItemUIElement _itemUIElementPrefab = null;
        [SerializeField] private UIItemDescriptionWindow _descriptionWindow = null;
        [SerializeField] private RectTransform _itemUIElementsContainer = null;
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;
        [SerializeField] private MousePointerMover _mousePointer = null;

        private List<ItemUIElement> _inventoryPresenterCells = new();

        private int _currentDraggingItemIndex = -1;

        public event Action<StackDataForUI> ItemDropped;
        
        public void InitializeInventoryPresenterWithCells(int cellCount)
        {
            _inventoryPresenterCells ??= new List<ItemUIElement>();

            for (var i = 0; i < cellCount; i++)
            {
                AddInventoryCell();
            }
        }

        public void UpdateData(StackDataForUI stackData)
        {
            if(stackData == null) return;
            var indexOfCell = _inventoryPresenterCells.FindIndex(cell => cell?.StackData?.ItemID == stackData?.ItemID && cell?.StackData?.StackIndex == stackData?.StackIndex);
            if(indexOfCell == -1) indexOfCell = _inventoryPresenterCells.FindIndex(cell => cell.IsEmpty == true);

            _inventoryPresenterCells[indexOfCell].InitializeWith(stackData);
        }

        private void AddInventoryCell()
        {
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
            if (!RectTransformUtility.RectangleContainsScreenPoint(_itemUIElementsContainer, Input.mousePosition))
            {
               ItemDropped?.Invoke(element.StackData);
               element.ResetData();
            }
            
            ResetSelection();
            ResetDraggingElement();
        }

        

        private void HandleSwap(ItemUIElement element)
        {
            if(element == null || _currentDraggingItemIndex == -1) return;

            var index = _inventoryPresenterCells.IndexOf(element);
            if(index < 0 || index >= _inventoryPresenterCells.Count)  return;

            var dataBuffer = _inventoryPresenterCells[_currentDraggingItemIndex].StackData;
            _inventoryPresenterCells[_currentDraggingItemIndex].InitializeWith(_inventoryPresenterCells[index].StackData);
            _inventoryPresenterCells[index].InitializeWith(dataBuffer);
        }

        private void HandleBeginDrag(ItemUIElement element)
        {
            if(element.IsEmpty) return;
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;
            _currentDraggingItemIndex = index;
            HandleItemSelection(element);
            CreateDraggedItem(element.StackData);
        }

        private void CreateDraggedItem(StackDataForUI item)
        {
            _mousePointer.Toggle(true);
            _mousePointer.SetData(item);
        }

        private void HandleItemSelection(ItemUIElement element)
        {
            ResetSelection();
            if(element.IsEmpty) return;
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;
            element.Select();
            _descriptionWindow.InitializeWith(element.StackData.ItemDatabase, element.StackData.ItemID);
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
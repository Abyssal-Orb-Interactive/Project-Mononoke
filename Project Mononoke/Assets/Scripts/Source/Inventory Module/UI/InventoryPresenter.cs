using System;
using System.Collections.Generic;
using System.Linq;
using Source.BuildingModule;
using Source.ItemsModule;
using Source.UI;
using UnityEngine;
using VContainer;

namespace Source.InventoryModule.UI
{
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private ItemUIElement _itemUIElementPrefab = null;
        [SerializeField] private UIItemDescriptionWindow _descriptionWindow = null;
        [SerializeField] private RectTransform _itemUIElementsContainer = null;
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;
        [SerializeField] private MousePointerMover _mousePointer = null;

        private List<ItemUIElement> _inventoryPresenterCells = null;
        private Inventory _inventory = null;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDraggingRequested;
        public event Action<int,int> OnSwapItemsRequested;

        private int _currentDraggingItemIndex = -1;

        [Inject] public void Initialize(Inventory inventory)
        {
            _inventory = inventory;
        } 

        public void PresentInventory()
        {
            _descriptionWindow.Reset();
            foreach(var item in _inventory)
            {
                AddInventoryCell();
                //_inventoryPresenterCells.Last().InitializeWith(item);       
            }
        }

        private void InitializeInventoryPresenterWithCells()
        {
            _inventoryPresenterCells ??= new();

            for (var i = 0; i < 10; i++)
            {
                AddInventoryCell();
            }
        }

        public void UpdateData(int itemIndex, IPickUpable item)
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
            ResetDraggingElement();
        }

        

        private void HandleSwap(ItemUIElement element)
        {
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;

            var itemBuffer = _inventoryPresenterCells[_currentDraggingItemIndex]._item;
            _inventoryPresenterCells[_currentDraggingItemIndex].InitializeWith(_inventoryPresenterCells[index]._item);
            _inventoryPresenterCells[index].InitializeWith(itemBuffer);
            OnSwapItemsRequested?.Invoke(_currentDraggingItemIndex, index);
        }

        private void HandleBeginDrag(ItemUIElement element)
        {
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;
            _currentDraggingItemIndex = index;
            HandleItemSelection(element);
            OnStartDraggingRequested?.Invoke(_currentDraggingItemIndex);
            CreateDraggedItem(element._item);
        }

        private void CreateDraggedItem(IPickUpable item)
        {
            _mousePointer.Toggle(true);
            _mousePointer.SetData(item);
        }

        private void HandleItemSelection(ItemUIElement element)
        {
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return;
            OnDescriptionRequested?.Invoke(index);
            element.Select();
            //_descriptionWindow.InitializeWith(element, element, element);
            ResetSelection();
        }

        private void ResetSelection()
        {
            _descriptionWindow.Reset();
            //ResetDraggingElement();
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

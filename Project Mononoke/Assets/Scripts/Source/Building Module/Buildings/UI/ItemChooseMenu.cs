using System;
using System.Collections.Generic;
using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using UnityEngine;
using VContainer;

namespace Source.BuildingModule.Buildings.UI
{
    public class ItemChooseMenu : MonoBehaviour
    {
        [SerializeField] private ItemUIElement _itemUIElementPrefab = null;
        [SerializeField] private RectTransform _itemUIElementsContainer = null;
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;
        
        private List<ItemUIElement> _inventoryPresenterCells = new();

        public event Action<InventoryPresenter.StackDataForUI> ItemSelected;

        [Inject]
        public void Initialize(ItemUIElement itemUIElementPrefab, RectTransform itemUIElementsContainer, OnGridObjectPlacer objectPlacer)
        {
            _itemUIElementPrefab = itemUIElementPrefab;
            _itemUIElementsContainer = itemUIElementsContainer;
            _objectPlacer = objectPlacer;
        }
        
        public void InitializeInventoryPresenterWithCells(int cellCount)
        {
            _inventoryPresenterCells ??= new List<ItemUIElement>();

            for (var i = 0; i < cellCount; i++)
            {
                AddItemCell();
            }
        }
        
        public void UpdateData(InventoryPresenter.StackDataForUI stackData)
        {
            if(stackData == null) return;
            var indexOfCell = _inventoryPresenterCells.FindIndex(cell => cell?.StackData?.ItemData.ID == stackData?.ItemData.ID && cell?.StackData?.StackIndex == stackData?.StackIndex);
            if(indexOfCell == -1) indexOfCell = _inventoryPresenterCells.FindIndex(cell => cell.IsEmpty == true);

            _inventoryPresenterCells[indexOfCell].InitializeWith(stackData);
        }

        public void UpdateMenu(List<InventoryPresenter.StackDataForUI> stacksData)
        {
            foreach (var cell in _inventoryPresenterCells)
            {
                cell.ResetData();
            }
            
            foreach (var stackData in stacksData)
            {
                UpdateData(stackData);
            }
        }

        private void AddItemCell()
        {
            var itemUIElement = _objectPlacer.PlaceObject(new ObjectPlacementInformation<ItemUIElement>(_itemUIElementPrefab, Vector3.zero ,Quaternion.identity, _itemUIElementsContainer));
            itemUIElement.ResetData();
            itemUIElement.OnItemLeftClicked += HandleItemSelection;
            _inventoryPresenterCells.Add(itemUIElement);
        }
        
        private void HandleItemSelection(ItemUIElement element)
        {
            ResetSelection();
            if (element.IsEmpty) return;
            var index = _inventoryPresenterCells.IndexOf(element);
            if(index == -1) return; 
            element.Select();
            ItemSelected?.Invoke(element.StackData);
        }
       
        private void ResetSelection()
        { 
            DeselectAllItems();
        }
       
        private void DeselectAllItems()
        { 
            foreach(var element in _inventoryPresenterCells)
            {
                element.Deselect();
            }
        }

        public void ToggleWith(bool signal)
        {
            gameObject.SetActive(signal);
        }
    }
}
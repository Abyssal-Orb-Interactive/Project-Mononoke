using System.Collections.Generic;
using System.Linq;
using Source.BuildingModule;
using Source.ItemsModule;
using UnityEngine;
using VContainer;

namespace Source.InventoryModule.UI
{
    public class InventoryPresenter : MonoBehaviour
    {
        [SerializeField] private ItemUIElement _itemUIElementPrefab = null;
        [SerializeField] private RectTransform _itemUIElementsContainer = null;
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;

        private List<ItemUIElement> _inventoryPresenterCells = null;
        private Inventory _inventory = null;

        [Inject] public void Initialize(Inventory inventory)
        {
            _inventory = inventory;
        } 

        public void PresentInventory()
        {
            foreach(var item in _inventory)
            {
                AddInventoryCell();
                _inventoryPresenterCells.Last().InitializeWith(item);       
            }
        }

        private void InitializeInventoryPresenterWithCells()
        {
            _inventoryPresenterCells ??= new();

            for (var i = 0; i < _inventory.ItemCapacity * 2; i++)
            {
                AddInventoryCell();
            }
        }

        private void AddInventoryCell()
        {
            _inventoryPresenterCells ??= new();
            var itemUIElement = _objectPlacer.PlaceObject(new ObjectPlacementInformation<ItemUIElement>(_itemUIElementPrefab, Vector3.zero ,Quaternion.identity, _itemUIElementsContainer));
            itemUIElement.Deactivate();
            _inventoryPresenterCells.Add(itemUIElement);
        }
    }
}

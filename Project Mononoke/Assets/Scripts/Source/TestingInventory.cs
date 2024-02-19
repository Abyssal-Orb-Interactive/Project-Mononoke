using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using Source.Character.Movement;

namespace Scripts.Source
{
    public class TestingInventory : MonoBehaviour
    {
        [SerializeField] private InventoryTableView _view = null;
        [SerializeField] private PickUpper _pickUpper = null;
        [SerializeField] private ItemView _item = null;
        [SerializeField] private ItemsDatabase _database = null;
        [SerializeField] private IsoCharacterMover _mover = null;

        private BuildingTool _hoe = null;

        private void Start()
        {
            var inventoryPresenter = new InventoryPresenter(_pickUpper.Inventory, _view);
            _hoe = new BuildingTool(2, _database, 0);
            _item.Initialize(_hoe);
        }

        private void Update() 
        {
          if (Input.GetKeyDown(KeyCode.B))
          {
              _hoe.UseMatterIn(_mover);
          }       
        }
    }
}

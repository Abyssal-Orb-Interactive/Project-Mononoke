using Base.Timers;
using Source.BuildingModule.Buildings;
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
        [SerializeField] private ItemsDatabase<ItemData> _database = null;
        [SerializeField] private IsoCharacterMover _mover = null;
        [SerializeField] private ItemsDatabase<SeedData> seedDatabase = null;
        [SerializeField] private Seedbed _seedbed;

        private BuildingTool _hoe = null;
        private TimeInvoker _timeInvoker = null;

        private void Start()
        {
            _timeInvoker = TimeInvoker.Instance;
            TimersFabric.Initialize(_timeInvoker);
            var inventoryPresenter = new InventoryPresenter(_pickUpper.Inventory, _view);
            _hoe = new BuildingTool(2, _database, 0);
            var seed = new Item<SeedData>(0, seedDatabase, 0);
            _item.Initialize(_hoe);
            _seedbed.Initialize(seed);
              
        }

        private void Update() 
        { 
            _timeInvoker.UpdateTimer();
            if (Input.GetKeyDown(KeyCode.B))
            {
                _hoe.UseMatterIn(_mover);
            }       
        }
    }
}

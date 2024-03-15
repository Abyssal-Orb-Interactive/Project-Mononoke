using Base.Timers;
using Source.BuildingModule;
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
        [SerializeField] private ItemView _itemViewPrefab = null;
        [SerializeField] private ItemsDatabase<ItemData> _database = null;
        [SerializeField] private IsoCharacterMover _mover = null;
        [SerializeField] private ItemsDatabase<SeedData> seedDatabase = null;
        [SerializeField] private Seedbed _seedbed;
        [SerializeField] private HandlingItemVisualizer _handlingItemVisualizer = null;
        [SerializeField] private OnGridObjectPlacer _placer;
        [SerializeField] private Transform _itemViewsContainer;
        
        private TimeInvoker _timeInvoker = null;

        private void Start()
        {
            _timeInvoker = TimeInvoker.Instance;
            TimersFabric.Initialize(_timeInvoker);
            ItemViewFabric.Initialize(_itemViewPrefab, _itemViewsContainer, _placer);
            ItemViewFabric.Create(new Item<ItemData>("Tomato", _database), new Vector3(-1,-1));
            ItemViewFabric.Create(new Item<ItemData>("Tomato", _database), new Vector3(-1,-1));
            ItemViewFabric.Create(new Item<ItemData>("Tomato", _database), new Vector3(-1,-1));
            var inventoryPresenter = new InventoryPresenter(_pickUpper.Inventory, _view);
            var seed = new Item<SeedData>("Onion", seedDatabase);
            _seedbed.Plant(seed);
            _handlingItemVisualizer.InitializeWith(_pickUpper.Manipulator);
        }

        private void Update() 
        { 
            _timeInvoker.UpdateTimer();
            if (Input.GetKeyDown(KeyCode.E))
            {
                _pickUpper.TryUseItemInManipulatorMatterIn(_mover);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _pickUpper.TryStashInInventory();
            }
        }
    }
}

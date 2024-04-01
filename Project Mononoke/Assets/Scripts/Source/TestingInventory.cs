using Base.Grid;
using Base.TileMap;
using Base.Timers;
using Source.BuildingModule;
using Source.BuildingModule.Buildings;
using Source.Character;
using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using Source.Character.Movement;
using Source.UI;
using UnityEngine.Tilemaps;

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
        [SerializeField] private CharacterLogicIsometric2DCollider _isometric2DCollider = null;
        [SerializeField] private Tilemap _tileMap = null;
        [SerializeField] private InteractiveObjectsFollower _follower = null;

        private TimeInvoker _timeInvoker = null;

        private void Start()
        {
            _timeInvoker = TimeInvoker.Instance;
            var inventory = new Inventory(100, 100);
            var manipulator = new Manipulator(5,3);
            var inventoryPresenter = new InventoryPresenter(inventory, _view);
            _pickUpper.Initialize(inventory,manipulator, inventoryPresenter);
            TimersFabric.Initialize(_timeInvoker);
            ItemViewFabric.Initialize(_itemViewPrefab, _itemViewsContainer, _placer);
            ItemViewFabric.Create(new Item<ItemData>("Tomato", _database), new Vector3(-1,-1));
            ItemViewFabric.Create(new Item<ItemData>("Tomato", _database), new Vector3(-1,-1));
            ItemViewFabric.Create(new Item<ItemData>("Tomato", _database), new Vector3(-1, -1));
            var seed = new Item<SeedData>("Onion", seedDatabase);
            _seedbed.Plant(seed);
            _handlingItemVisualizer.InitializeWith(_pickUpper.Manipulator);
            var tileMapWrapper = new UnityTileMapWrapper(_tileMap);
            var tileCollectionAnalyzer = new TileCollectionAnalyzer(tileMapWrapper);
            var grid = new GroundGrid(tileCollectionAnalyzer);
            var gridAnalyzer = new GridAnalyzer(_mover, grid);
            _isometric2DCollider.Initialize(gridAnalyzer);
            _follower.Initialize(_isometric2DCollider);
        }

        private void Update() 
        { 
            _isometric2DCollider.FrameByFrameCalculate();
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

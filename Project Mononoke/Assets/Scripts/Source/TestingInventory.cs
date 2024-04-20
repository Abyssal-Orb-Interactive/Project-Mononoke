using Base.DIContainer;
using Base.Grid;
using Base.Input;
using Base.TileMap;
using Base.Timers;
using Pathfinding;
using Source.BuildingModule;
using Source.BuildingModule.Buildings;
using Source.BuildingModule.Buildings.UI;
using Source.Character;
using Source.Character.AI;
using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using Source.Character.Movement;
using Source.Character.Visual;
using Source.UI;
using UnityEngine.Tilemaps;
using VContainer;

namespace Scripts.Source
{
    public class TestingInventory : MonoBehaviour
    {
        [SerializeField] private PickUpper _pickUpper = null;
        [SerializeField] private ItemView _itemViewPrefab = null;
        [SerializeField] private ItemsDatabase<ItemData> _database = null;
        [SerializeField] private IsoCharacterMover _mover = null;
        [SerializeField] private ItemsDatabase<SeedData> _seedDatabase = null;
        [SerializeField] private HandlingItemVisualizer _handlingItemVisualizer = null;
        [SerializeField] private OnGridObjectPlacer _placer;
        [SerializeField] private Transform _itemViewsContainer;
        [SerializeField] private CharacterLogicIsometric2DCollider _isometric2DCollider = null;
        [SerializeField] private InteractiveObjectsFollower _follower = null;
        [SerializeField] private GameLifetimeScope _lifetimeScope = null;
        [SerializeField] private ItemsDatabase<ItemData> _toolsDatabase;
        [SerializeField] private PathfinderAI _ai = null;
        [SerializeField] private Transform _target = null;
        [SerializeField] private IsoCharacterMover _aiMover = null;
        [SerializeField] private TargetMover _targetMover = null;
        [SerializeField] private CharacterSpiteAnimationPlayer _animationPlayer = null;

        private TimeInvoker _timeInvoker = null;

        private void Start()
        {
            _timeInvoker = TimeInvoker.Instance;
            var manipulator = new Manipulator(5,3);
            _pickUpper.Initialize( _lifetimeScope.Container.Resolve<Inventory>(),manipulator, _lifetimeScope.Container.Resolve<InventoryPresenter>());
            TimersFabric.Initialize(_timeInvoker);
            ItemViewFabric.Initialize(_itemViewPrefab, _itemViewsContainer, _placer);
            _handlingItemVisualizer.InitializeWith(_pickUpper.Manipulator);
            _follower.Initialize(_isometric2DCollider);
            _lifetimeScope.Container.Resolve<OnGridBuilder>();
            var handler = new BuildingsInteractionsRequestsHandler();
            handler.AddRequester(_follower);
            _seedDatabase.TryGetItemDataBy("Onion", out var seedData);
            ItemViewFabric.Create(new Item(seedData), new Vector3(0.5f, 1));
            ItemViewFabric.Create(new Item(seedData), new Vector3(0.5f, 1));
            ItemViewFabric.Create(new Item(seedData), new Vector3(0.5f, 1));
            _toolsDatabase.TryGetItemDataBy("Hoe", out var toolData);
            ItemViewFabric.Create(new Item(toolData), new Vector3(-1, -0.5f));
            var gridGraphNodesWalkableUpdater = new GridGraphNodesWalkableUpdater();
            gridGraphNodesWalkableUpdater.UpdateGridGraphUsing(_lifetimeScope.Container.Resolve<GroundGrid>());
            _aiMover.Initialize(_lifetimeScope.Container.Resolve<GroundGrid>(), new InputHandler(_ai));
            _animationPlayer.Initialize(_aiMover);
            _ai.StartFollowing(_target.position);
            _targetMover.PositionChanged += OnPositionChanged;
        }

        private void OnPositionChanged(Vector3 newPosition)
        {
            _ai.StartFollowing(_target.position);
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

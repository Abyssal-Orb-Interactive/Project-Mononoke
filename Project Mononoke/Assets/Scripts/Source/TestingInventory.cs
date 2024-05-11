using Base.DIContainer;
using Base.Grid;
using Base.Input;
using Base.Timers;
using Source.BuildingModule;
using Source.BuildingModule.Buildings;
using Source.BuildingModule.Buildings.UI;
using Source.Character;
using Source.Character.AI;
using Source.Character.Minions_Manager;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using Source.Character.Movement;
using Source.Character.Visual;
using Source.Formations;
using Source.InventoryModule.UI;
using Source.UI;
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
        [SerializeField] private IsoCharacterMover _aiMover = null;
        [SerializeField] private CharacterLogicIsometric2DCollider _aiCollider = null;
        [SerializeField] private PickUpper _aiPickUpper = null;
        [SerializeField] private CharacterSpiteAnimationPlayer _aiAnimationPlayer = null;
        [SerializeField] private MinionsTargetPositionCoordinator _minionsTargetPositionCoordinator = null;
        [SerializeField] private HandlingItemVisualizer _aiHandlingItemVisualizer = null;
        [SerializeField] private GameObject _minionPrefab = null;
        [SerializeField] private TestArmy _army = null;
        [SerializeField] private FormationPositionsHolder _formationPositionsHolder = null;
        [SerializeField] private FormationGizmosRenderer _formationGizmos = null;

        private TimeInvoker _timeInvoker = null;

        private void Start()
        {
            _minionsTargetPositionCoordinator.Initialize(_mover);
            MinionsFactory.Initialize(_minionPrefab, _placer, new GameObject("Army").transform, _lifetimeScope, _minionsTargetPositionCoordinator);
            _timeInvoker = TimeInvoker.Instance;
            var manipulator = new Manipulator(5,3);
            _pickUpper.Initialize( _lifetimeScope.Container.Resolve<Inventory>(),manipulator);
            _pickUpper.SubscribeOnUIUpdates(_lifetimeScope.Container.Resolve<InventoryPresenter>());
            TimersFabric.Initialize(_timeInvoker);
            ItemViewFabric.Initialize(_itemViewPrefab, _itemViewsContainer, _placer);
            _handlingItemVisualizer.InitializeWith(_pickUpper.Manipulator);
            _follower.Initialize(_isometric2DCollider);
            _lifetimeScope.Container.Resolve<OnGridBuilder>();
            var handler = new BuildingsInteractionsRequestsHandler();
            handler.AddRequester(_follower);
            _seedDatabase.TryGetItemDataBy("Onion", out var seedData);
            ItemViewFabric.Create(new Item(seedData), new Vector3(0.5f, 1));
            ItemViewFabric.Create(new Item(seedData), new Vector3(0.5f, 0.5f));
            ItemViewFabric.Create(new Item(seedData), new Vector3(0.5f, 0.25f));
            _toolsDatabase.TryGetItemDataBy("Hoe", out var toolData);
            ItemViewFabric.Create(new Item(toolData), new Vector3(-1, -0.5f));
            var gridGraphNodesWalkableUpdater = new GridGraphNodesWalkableUpdater();
            gridGraphNodesWalkableUpdater.UpdateGridGraphUsing(_lifetimeScope.Container.Resolve<GroundGrid>());
            _aiMover.Initialize(_lifetimeScope.Container.Resolve<GroundGrid>(), new InputHandler(_ai));
            _aiAnimationPlayer.Initialize(_aiMover);
            _aiCollider.Initialize(new GridAnalyzer(_aiMover, _lifetimeScope.Container.Resolve<GroundGrid>()));
            var aiManipulator = new Manipulator(5, 5);
            var aiInventory = new Inventory(1, 1);
            _aiPickUpper.Initialize(aiInventory, aiManipulator);
            _aiHandlingItemVisualizer.InitializeWith(aiManipulator);
            var collidersHolder = new CollidersHolder(_aiCollider, _aiPickUpper);
            _ai.Initialize(_minionsTargetPositionCoordinator, collidersHolder, _aiPickUpper);
            var formation = new Wedge(3);
            _formationPositionsHolder.Initialize(formation, _placer, _mover);
            _formationGizmos.Initialize(formation);
        }

        private void Update() 
        { 
            _isometric2DCollider.FrameByFrameCalculate();
            _aiCollider.FrameByFrameCalculate();
            
            _timeInvoker.UpdateTimer();
            if (Input.GetKeyDown(KeyCode.E))
            {
                _pickUpper.TryUseItemInManipulatorMatterIn(_mover);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                _pickUpper.TryStashInPickUpperInventory();
            }
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                _army.DispatchUnit();
                _minionsTargetPositionCoordinator.ChangeTargetPosition();
            }
        }
    }
}

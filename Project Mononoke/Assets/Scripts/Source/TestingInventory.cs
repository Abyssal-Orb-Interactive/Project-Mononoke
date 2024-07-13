using Base.Databases;
using Base.DIContainer;
using Base.GameLoop;
using Base.Grid;
using Base.Timers;
using Source.BattleSystem;
using Source.BattleSystem.UI;
using Source.BuildingModule;
using Source.BuildingModule.Buildings;
using Source.BuildingModule.Buildings.UI;
using Source.BuildingModule.Buildings.Visual;
using Source.Character;
using Source.Character.CharacterFabrics;
using Source.Character.Database;
using Source.Character.Minions_Manager;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using Source.Character.Movement;
using Source.Formations;
using Source.InventoryModule.UI;
using Source.UI;
using VContainer;
using Container = Source.BuildingModule.Buildings.Container;

namespace Scripts.Source
{
    public class TestingInventory : MonoBehaviour
    {
        [SerializeField] private PickUpper _pickUpper = null;
        [SerializeField] private ItemView _itemViewPrefab = null;
        [SerializeField] private IsoCharacterMover _mover = null;
        [SerializeField] private DatabaseSO<SeedData> _seedDatabase = null;
        [SerializeField] private HandlingItemVisualizer _handlingItemVisualizer = null;
        [SerializeField] private OnGridObjectPlacer _placer;
        [SerializeField] private Transform _itemViewsContainer;
        [SerializeField] private CharacterLogicIsometric2DCollider _isometric2DCollider = null;
        [SerializeField] private InteractiveObjectsFollower _follower = null;
        [SerializeField] private GameLifetimeScope _lifetimeScope = null;
        [SerializeField] private DatabaseSO<ItemData> _toolsDatabase;
        [SerializeField] private MinionsTargetPositionCoordinator _minionsTargetPositionCoordinator = null;
        [SerializeField] private GameObject _minionPrefab = null;
        [SerializeField] private TestArmy _army = null;
        [SerializeField] private FormationPositionsHolder _formationPositionsHolder = null;
        [SerializeField] private StatsHolder _playersStatsHolder = null;
        [SerializeField] private CollidersHolder _collidersHolder = null;
        [SerializeField] private GameObject _healthBarPrefab = null;
        [SerializeField] private GameObject _surroundedHealthBarPrefab = null;
        [SerializeField] private HealthBarsCanvas _healthBarsCanvas = null;
        [SerializeField] private Container _container = null;
        [SerializeField] private Damageable _damageableC = null;
        [SerializeField] private InventoryMenu _containersMenu = null;
        [SerializeField] private InventoryTableView _containersInventory = null;
        [SerializeField] private InventoryTableView _playersInventory = null;
        [SerializeField] private ItemChooseMenu _chooseMenu = null;
        [SerializeField] private PresetBuildingsInGridInitializer _presetBuildingsInGridInitializer = null;
        [SerializeField] private RectTransform _containersUIContent = null;
        [SerializeField] private RectTransform _playersUIContent = null;
        [SerializeField] private InventoryMenuSwitcher _inventoryMenuSwitcher = null;
        [SerializeField] private Damageable _playerDamagable = null;
        [SerializeField] private CharactersDatabase _charactersDatabase = null;
        [SerializeField] private Transform _enemiesHolder = null;
        [SerializeField] private DamageArea _damageArea = null;
        [SerializeField] private GameLoop _gameLoop = null;
        [SerializeField] private DatabaseSO<ItemData> _trashItemDatabase = null;
        [SerializeField] private Transform _minionsHolder = null;
        [SerializeField] private BarrelSpriteAnimationPlayer _barrelSpriteAnimationPlayer = null;
        [SerializeField] private HealthBar _healthBar = null;
        [SerializeField] private ItemDropper _itemDropper = null;
        [SerializeField] private ItemLauncher _itemLauncher = null;
        private TimeInvoker _timeInvoker = null;

        private void Start()
        {
            _minionsTargetPositionCoordinator.Initialize(_mover);
            MinionsFactory.Initialize(_minionPrefab, _placer, new GameObject("Army").transform, _lifetimeScope);
            _timeInvoker = TimeInvoker.Instance;
            var manipulator = new Manipulator(5,3);
            _collidersHolder.Initialize(_isometric2DCollider);
            _pickUpper.Initialize( _lifetimeScope.Container.Resolve<Inventory>(),manipulator, _collidersHolder);
            _pickUpper.SubscribeOnUIUpdates(_lifetimeScope.Container.Resolve<InventoryPresenter>());
            TimersFabric.Initialize(_timeInvoker);
            ItemViewFabric.Initialize(_itemViewPrefab, _itemViewsContainer, _placer);
            _handlingItemVisualizer.InitializeWith(_pickUpper.Manipulator);
            _follower.Initialize(_isometric2DCollider);
            _lifetimeScope.Container.Resolve<OnGridBuilder>();
            var handler = new BuildingsInteractionsRequestsHandler();
            handler.AddRequester(_follower);
            var gridGraphNodesWalkableUpdater = new GridGraphNodesWalkableUpdater();
            gridGraphNodesWalkableUpdater.UpdateGridGraphUsing(_lifetimeScope.Container.Resolve<GroundGrid>());
            _healthBarsCanvas.Initialize(_healthBarPrefab, _surroundedHealthBarPrefab);
            var formation = new Wedge(3);
            _formationPositionsHolder.Initialize(formation, _placer, _mover);
            _damageableC.Initialize(5, Fractions.Neutral);
            var containersInventory = new Inventory();
            _container.Initialize(containersInventory, _containersMenu);
            var containersInventoryPresenter =
                new InventoryPresenter(containersInventory, _containersInventory, _chooseMenu);
            var containersDropProcessor = new UIDropsProcessor(containersInventoryPresenter,
                _playersUIContent, _lifetimeScope.Container.Resolve<Inventory>(), _itemDropper);
            var playerInventoryPresenter = new InventoryPresenter(_lifetimeScope.Container.Resolve<Inventory>(),
                _playersInventory, _chooseMenu);
            var playerDropProcessor = new UIDropsProcessor(playerInventoryPresenter,
                _containersUIContent, containersInventory, _itemDropper);
            var inventoryPlayersDropsProcessor = new UIDropsProcessor(
                _lifetimeScope.Container.Resolve<InventoryPresenter>(), _containersUIContent,
                _lifetimeScope.Container.Resolve<Inventory>(), _itemDropper);
            _containersInventory.InitializeInventoryPresenterWithCells(10);
            _playersInventory.InitializeInventoryPresenterWithCells(10);
            _seedDatabase.Initialize(new SeedDataValidator());
            _toolsDatabase.Initialize(new ItemDataValidator());
            _trashItemDatabase.Initialize(new ItemDataValidator());
            _seedDatabase.TryGetItemDataBy("Onion Seed", out var onionSeedData);
            _seedDatabase.TryGetItemDataBy("Carrot Seed", out var carrotSeedData);
            _seedDatabase.TryGetItemDataBy("Tomato Seed", out var tomatoSeedData);
            _toolsDatabase.TryGetItemDataBy("Hoe", out var toolData);
            _container.Add(new Item(onionSeedData));
            _container.Add(new Item(carrotSeedData));
            _container.Add(new Item(tomatoSeedData));
            _container.Add(new Item(toolData));
            _presetBuildingsInGridInitializer.Initilize(_lifetimeScope.Container.Resolve<GroundGrid>());
            _inventoryMenuSwitcher.Initialize();
            _playerDamagable.Initialize(10, Fractions.Plodomorphs);
            _playersStatsHolder.Initialize(_playerDamagable, 1, Fractions.Plodomorphs);
            _charactersDatabase.Initialize(new CharacterDataValidator());
            if (_charactersDatabase.TryGetItemDataBy("Monkey", out var mageData))
            {
                var mageFabric = new CharactersFabric(mageData, _lifetimeScope, _placer, _enemiesHolder, _gameLoop, _healthBarsCanvas);
                mageFabric.CreateAt(Vector3.zero);
            }

            if (_charactersDatabase.TryGetItemDataBy("Gray Minion", out var minionData))
            {
                var unitFabric = new CharactersFabric(minionData, _lifetimeScope, _placer, _minionsHolder, _gameLoop, _healthBarsCanvas);
                for (var i = 0; i < 1; i++)
                {
                   var unit = unitFabric.CreateAt(_playersStatsHolder.transform.position);
                   _army.TryAddToArmy(unit, _minionsHolder);
                }
            }

            var playerUpdater = new CharacterComponentsUpdater(_mover, _isometric2DCollider);
            _gameLoop.RegisterUpdatable(playerUpdater);
            _damageArea.Initialize(_playersStatsHolder);
            _barrelSpriteAnimationPlayer.Initialize();
            _healthBar.Initialize(_playerDamagable);
            _itemLauncher.Initialize(_lifetimeScope.Container.Resolve<GroundGrid>());
            _itemDropper.Initializie();
        }

        private void Update() 
        { 
            _healthBarsCanvas.UpdateAllHealthBarsPositions();

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

            if (Input.GetKeyDown(KeyCode.F))
            {
                _army.ReturnAllDispatchedUnitsToFormation();
            }

            if (Input.GetMouseButtonDown(0))
            {
                _playersStatsHolder.TriggerAttack();
            }
        }
    }
}

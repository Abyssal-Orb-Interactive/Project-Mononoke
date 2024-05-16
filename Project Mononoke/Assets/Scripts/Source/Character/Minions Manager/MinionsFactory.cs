using Base.Grid;
using Base.Input;
using Source.BattleSystem;
using Source.BuildingModule;
using Source.Character.AI;
using Source.Character.Movement;
using Source.Character.Visual;
using Source.InventoryModule;
using Source.PickUpModule;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Source.Character.Minions_Manager
{
    public static class MinionsFactory
    {
        private static GameObject _minionPrefab = null;
        private static OnGridObjectPlacer _objectPlacer = null;
        private static Transform _minionsHolder = null;
        private static LifetimeScope _lifetimeScope = null;
        private static MinionsTargetPositionCoordinator _minionsTargetPositionCoordinator = null;
        
        [Inject] public static void Initialize(GameObject minionPrefab, OnGridObjectPlacer objectPlacer, Transform minionsHolder, LifetimeScope lifetimeScope, MinionsTargetPositionCoordinator minionsTargetPositionCoordinator)
        {
            _minionPrefab = minionPrefab;
            _objectPlacer = objectPlacer;
            _minionsHolder = minionsHolder;
            _lifetimeScope = lifetimeScope;
            _minionsTargetPositionCoordinator = minionsTargetPositionCoordinator;
        }

        public static GameObject Create(Vector3 worldPosition)
        {
            var minion = _objectPlacer.PlaceObject(new ObjectPlacementInformation<GameObject>(_minionPrefab, worldPosition,
                Quaternion.identity, _minionsHolder));
            var minionAI = minion.GetComponentInChildren<PathfinderAI>();
            var minionMover = minion.GetComponentInChildren<IsoCharacterMover>();
            var minionCollider = minion.GetComponentInChildren<CharacterLogicIsometric2DCollider>();
            var minionPickUpper = minion.GetComponentInChildren<PickUpper>();
            var minionAnimationPlayer = minion.GetComponentInChildren<CharacterSpiteAnimationPlayer>();
            var minionHandlingItemVisualizer = minion.GetComponentInChildren<HandlingItemVisualizer>();
            var statsHolder = minion.GetComponentInChildren<StatsHolder>();
            minionMover.Initialize(_lifetimeScope.Container.Resolve<GroundGrid>(), new InputHandler(minionAI));
            minionAnimationPlayer.Initialize(minionMover);
            minionCollider.Initialize(new GridAnalyzer(minionMover, _lifetimeScope.Container.Resolve<GroundGrid>()));
            var minionManipulator = new Manipulator(5, 5);
            var minionInventory = new Inventory(1, 1);
            minionPickUpper.Initialize(minionInventory, minionManipulator);
            minionHandlingItemVisualizer.InitializeWith(minionManipulator);
            var collidersHolder = new CollidersHolder(minionCollider, minionPickUpper);
            statsHolder.Initialize(3,1, Fractions.Plodomorphs);
            minionAI.Initialize(_minionsTargetPositionCoordinator, collidersHolder, minionPickUpper, statsHolder);
            return minion;
        }
    }
}
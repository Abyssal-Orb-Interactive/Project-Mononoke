using System;
using System.Text;
using Base.DIContainer;
using Base.GameLoop;
using Base.Grid;
using Base.Input;
using Source.BattleSystem;
using Source.BattleSystem.UI;
using Source.BuildingModule;
using Source.Character.AI;
using Source.Character.AI.Areas;
using Source.Character.AI.BattleAI;
using Source.Character.AI.BattleAI.Behaviours.EnemyInDamageAreBehaviours;
using Source.Character.AI.BattleAI.Behaviours.EnemyInSearchAreaBehaviours;
using Source.Character.Database;
using Source.Character.Movement;
using Source.Character.Visual;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using VContainer;
using Object = UnityEngine.Object;

namespace Source.Character.CharacterFabrics
{
    public class CharactersFabric
    {
        private readonly CharacterData _characterData = null;
        private readonly GameLifetimeScope _scope = null;
        private readonly OnGridObjectPlacer _placer = null;
        private readonly Transform _charactersHolder = null;
        private readonly GameLoop _gameLoop = null;
        private readonly HealthBarsCanvas _healthBarsCanvas = null;

        public CharactersFabric(CharacterData characterData, GameLifetimeScope scope, OnGridObjectPlacer placer, Transform charactersHolder, GameLoop gameLoop, HealthBarsCanvas healthBarsCanvas)
        {
            _characterData = characterData;
            _scope = scope;
            _placer = placer;
            _charactersHolder = charactersHolder;
            _gameLoop = gameLoop;
            _healthBarsCanvas = healthBarsCanvas;
        }

        public GameObject CreateAt(Vector3 worldPosition)
        {
            var characterGameObject = _placer.PlaceObject(new ObjectPlacementInformation<GameObject>(_characterData.Prefab, worldPosition,
                Quaternion.identity, _charactersHolder));
            var errorsStringBuilder = new StringBuilder();
            if (!TryGetComponentInGameObjectHierarchy<IsoCharacterMover>(characterGameObject, out var mover)) errorsStringBuilder.AppendLine("All characters must have IsoCharacterMover component");
            if (!TryGetComponentInGameObjectHierarchy<CharacterLogicIsometric2DCollider>(characterGameObject, out var isometric2DCollider)) errorsStringBuilder.AppendLine("All characters must have CharacterLogicIsometric2DCollider component");
            if (!TryGetComponentInGameObjectHierarchy<PickUpper>(characterGameObject, out var pickUpper)) errorsStringBuilder.AppendLine("All characters must have PickUpper component");
            if (!TryGetComponentInGameObjectHierarchy<CollidersHolder>(characterGameObject, out var collidersHolder)) errorsStringBuilder.AppendLine("All characters must have CollidersHolder component");
            if (!TryGetComponentInGameObjectHierarchy<StatsHolder>(characterGameObject, out var statsHolder)) errorsStringBuilder.AppendLine("All characters must have StatsHolder component");
            if (!TryGetComponentInGameObjectHierarchy<Damageable>(characterGameObject, out var damageable)) errorsStringBuilder.AppendLine("All characters must have Damageable component");
            if (!TryGetComponentInGameObjectHierarchy<PathfinderAI>(characterGameObject, out var ai)) errorsStringBuilder.AppendLine("All characters must have PathfinderAI component");
            if (!TryGetComponentInGameObjectHierarchy<CharacterSpiteAnimationPlayer>(characterGameObject, out var animationPlayer)) errorsStringBuilder.AppendLine("All characters must have CharacterSpiteAnimationPlayer component");
            if (!TryGetComponentInGameObjectHierarchy<HandlingItemVisualizer>(characterGameObject, out var handlingItemVisualizer)) errorsStringBuilder.AppendLine("All characters must have HandlingItemVisualizer component");
            if (!TryGetComponentInGameObjectHierarchy<AISearch2DTriggerArea>(characterGameObject, out var searchAreaTrigger)) errorsStringBuilder.AppendLine("All characters must have AISearchAreaTrigger component");
            if (!TryGetComponentInGameObjectHierarchy<BattleAI>(characterGameObject, out var battleAI)) errorsStringBuilder.AppendLine("All characters must have BattleAI component");
            if (!TryGetComponentInGameObjectHierarchy<ItemLauncher>(characterGameObject, out var itemLauncher)) errorsStringBuilder.AppendLine("All characters must have ItemLauncher component");
            if (!TryGetComponentInGameObjectHierarchy<ItemDropper>(characterGameObject, out var itemDropper)) errorsStringBuilder.AppendLine("All characters must have ItemDropper component");
            if (!TryCreateEnemyInDamageAreaBehaviour(_characterData.EnemyInDamageAreaBehaviour, characterGameObject,
                    out var enemyInDamageAreaBehaviour, out var enemyInDamageAreaBehaviourCreationError)) errorsStringBuilder.AppendLine(enemyInDamageAreaBehaviourCreationError);
            if (!TryCreateEnemyInSearchAreaBehaviour(_characterData.EnemyInSearchAreaBehaviour, characterGameObject,
                    out var enemyInSearchAreaBehaviour, out var enemyInSearchAreaBehaviourCreationError))
                errorsStringBuilder.AppendLine(enemyInSearchAreaBehaviourCreationError); 
            var errors = errorsStringBuilder.ToString();

            if (errors.Length > 0)
            {
                Debug.LogError(errors);
                Object.Destroy(characterGameObject);
                return null;
            }
            var grid = _scope.Container.Resolve<GroundGrid>();
            ai.Initialize(collidersHolder, pickUpper, statsHolder);
            mover.Initialize(grid, new InputHandler(ai));
            itemLauncher.Initialize(grid);
            itemDropper.Initializie();
            animationPlayer.Initialize(mover);
            isometric2DCollider.Initialize(new GridAnalyzer(mover, grid));
            collidersHolder.Initialize(isometric2DCollider);
            var manipulator = new Manipulator(_characterData.ManipulatorStrength, _characterData.ManipulatorCapacity);
            var inventory = new Inventory(_characterData.InventoryWeightCapacity, _characterData.InventoryVolumeCapacity);
            pickUpper.Initialize(inventory, manipulator, collidersHolder);
            handlingItemVisualizer.InitializeWith(manipulator);
            damageable.Initialize(_characterData.HP, _characterData.Fraction);
            statsHolder.Initialize(damageable, _characterData.UnarmedDamage, _characterData.Fraction);
            battleAI.Initialize(ai, statsHolder, enemyInDamageAreaBehaviour, enemyInSearchAreaBehaviour);

            var updatable = new CharacterComponentsUpdater(mover, isometric2DCollider);
            _gameLoop.RegisterUpdatable(updatable);
                //if(damageable.Fraction == Fractions.Lesoviks) _healthBarsCanvas.AddHealthBarTo(damageable);
            return characterGameObject;
        }

        private bool TryGetComponentInGameObjectHierarchy<T>(GameObject root, out T component) where T : MonoBehaviour
        {
            if (root.TryGetComponent(out component))
            {
                return true;
            }

            return (component = root.GetComponentInChildren<T>()) != null;
        }

        private bool TryCreateEnemyInDamageAreaBehaviour(EnemyInDamageAreaBehaviours behaviourType,
            GameObject character, out IEnemyInDamageAreaBehaviour behaviour, out string error)
        {
            IDamager damager = null;
            ProjectileLauncher projectileLauncher = null;
            PathfinderAI ai = null;
            
            switch (behaviourType)
            {
                case EnemyInDamageAreaBehaviours.Hit:
                    error = "";
                    if (!character.TryGetComponent(out damager))
                    {
                        error += "For hit behaviour character must have IDamager component";
                        behaviour = null;
                        return false;
                    }
                    if (!character.TryGetComponent(out ai))
                    {
                        error += "For shoot behaviour character must have PathfinderAI component";
                    }

                    error = null;
                    behaviour = new HitBehaviour(damager);
                    return true;
                case EnemyInDamageAreaBehaviours.HitWithSurrounding:
                    error = "";
                    if (!character.TryGetComponent(out damager))
                    {
                        error += "For hit with surrounding behaviour character must have IDamager component";
                        behaviour = null;
                        return false;
                    }
                    if (!character.TryGetComponent(out ai))
                    {
                        error += "For shoot behaviour character must have PathfinderAI component";
                    }
                    error = null;
                    behaviour = new HitWithSurroundingBehaviour(damager, _healthBarsCanvas);
                    return true;
                case EnemyInDamageAreaBehaviours.Shoot:
                    error = "";
                    if (!character.TryGetComponent(out damager))
                    {
                        error += "For shoot behaviour character must have IDamager component\n";
                    }
                    if (!character.TryGetComponent(out projectileLauncher))
                    {
                        error += "For shoot behaviour character must have ProjectileLauncher component\n";
                    }
                    if (!character.TryGetComponent(out ai))
                    {
                        error += "For shoot behaviour character must have PathfinderAI component";
                    }

                    if (error != "")
                    {
                        behaviour = null;
                        return false;
                    }
                    error = null;
                    behaviour = new ShootBehaviour(projectileLauncher, ai, damager);
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(behaviourType), behaviourType, null);
            }
        }

        private static bool TryCreateEnemyInSearchAreaBehaviour(EnemyInSearchAreaBehaviours behaviourType,
            GameObject character, out IEnemyInSearchAreaBehaviour behaviour, out string error)
        {
            IDamager damager = null;
            PathfinderAI ai = null;
            
            switch (behaviourType)
            {
                case EnemyInSearchAreaBehaviours.Follow:
                    error = "";
                    if (!character.TryGetComponent(out damager))
                    {
                        error += "For follow behaviour character must have IDamager component\n";
                    }
                    if (!character.TryGetComponent(out ai))
                    {
                        error += "For follow behaviour character must have PathfinderAI component";
                    }

                    if (error != "")
                    {
                        behaviour = null;
                        return false;
                    }

                    error = null;
                    behaviour = new FollowBehaviour(damager, ai);
                    return true;
                case EnemyInSearchAreaBehaviours.Kite:
                    error = "";
                    if (!character.TryGetComponent(out damager))
                    {
                        error += "For follow behaviour character must have IDamager component\n";
                    }
                    if (!character.TryGetComponent(out ai))
                    {
                        error += "For follow behaviour character must have PathfinderAI component";
                    }

                    if (error != "")
                    {
                        behaviour = null;
                        return false;
                    }

                    error = null;
                    behaviour = new KiteBehaviour(damager, ai);
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(behaviourType), behaviourType, null);
            }
        }
    }
}
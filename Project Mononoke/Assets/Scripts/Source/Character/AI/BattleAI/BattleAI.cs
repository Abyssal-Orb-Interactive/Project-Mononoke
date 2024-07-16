using System;
using System.Collections.Generic;
using Base.Timers;
using Base.Math;
using Base.Input;
using Source.BattleSystem;
using Source.BattleSystem.Visual;
using Source.Character.AI.Areas;
using Source.Character.Movement;
using UnityEngine;

namespace Source.Character.AI.BattleAI
{
    public class BattleAI : MonoBehaviour, IDisposable
    {
        [SerializeField] private AISearch2DTriggerArea _search2DTriggerArea = null;
        [SerializeField] private CloseCombat2DTriggerArea _closeCombat2DTriggerArea = null;
        [SerializeField] private float _attackCooldown = 0f;
        [SerializeField] private AttackTrigger _attackTrigger = null;
        [SerializeField] private PathfinderAI _ai = null;
        
        private IDamager _damager = null;
        private IEnemyInSearchAreaBehaviour _enemyInSearchAreaBehaviour;
        private IEnemyInDamageAreaBehaviour _enemyInDamageAreaBehaviour;
        private Timer _attackCooldownTimer = null;

        private IsoCharacterMover _mover = null;
        
        private HashSet<IDamageable> _inCloseCombatAreaDamagables = null;
        
        private IDamageable _closestEnenmy = null;
        private float _distanceToClosestEnemy = float.MaxValue;
        private MovementDirection _currentDirection = MovementDirection.East;

        public event Action ClosestEnemyDeath, ClosestEnemyFound = null;

        public bool HasEnemies => _inCloseCombatAreaDamagables.Count > 0;

        public void Initialize(PathfinderAI ai, IDamager damager, IEnemyInDamageAreaBehaviour enemyInDamageAreaBehaviour, IEnemyInSearchAreaBehaviour enemyInSearchAreaBehaviour)
        {
            _ai = ai;
            _damager = damager;
            if (TryGetComponent<IsoCharacterMover>(out var mover))
            {
                _mover = mover;
                _mover.MovementChanged += OnDamagerMove;
            }
            InitializeBehaviours(enemyInDamageAreaBehaviour, enemyInSearchAreaBehaviour);
        }

        private void OnDamagerMove(object sender, IsoCharacterMover.MovementActionEventArgs e)
        {
            
        }

        public void InitializeBehaviours(IEnemyInDamageAreaBehaviour enemyInDamageAreaBehaviour,
            IEnemyInSearchAreaBehaviour enemyInSearchAreaBehaviour)
        {
            _enemyInSearchAreaBehaviour = enemyInSearchAreaBehaviour;
            _enemyInDamageAreaBehaviour = enemyInDamageAreaBehaviour;
            
            _inCloseCombatAreaDamagables = new HashSet<IDamageable>();
            
            StartListeningAreasSignals();
        }
        
        public void StartListeningAreasSignals()
        {
            _search2DTriggerArea.TargetEnteredInArea += OnTargetEnteredInSearch2DTriggerArea;
            _closeCombat2DTriggerArea.TargetEnteredInArea += OnTargetEnteredInCloseCombat;
            _closeCombat2DTriggerArea.TargetExitFromArea += OnTargetExitFromArea;
        }

        private void OnTargetExitFromArea(IDamageable enemy)
        {
            if(!_inCloseCombatAreaDamagables.Contains(enemy)) return;
            _inCloseCombatAreaDamagables.Remove(enemy);
            if (_closestEnenmy == enemy)
            {
                _closestEnenmy = null;
                _distanceToClosestEnemy = float.MaxValue;
            }
            var component = enemy as MonoBehaviour;
            if(component == null) return;
            if (component.TryGetComponent<IsoCharacterMover>(out var mover)) mover.MovementChanged -= OnTargetMoved;
            enemy.Death -= OnEnemyDeath;
        }

        private void OnTargetEnteredInCloseCombat(IDamageable enemy)
        {
            if(_inCloseCombatAreaDamagables.Contains(enemy)) return;
            if(enemy.Fraction == _damager.Fraction) return;
            _ai.StopFollowing();
            _inCloseCombatAreaDamagables.Add(enemy);

            var closestEnemyBuffer = _closestEnenmy;
            RecalculateInCloseCombatClosestEnemy();
            if (closestEnemyBuffer == _closestEnenmy) return;
            var component = enemy as MonoBehaviour;
            if(component == null) return;
                
            if (component.TryGetComponent<IsoCharacterMover>(out var mover)) mover.MovementChanged += OnTargetMoved;
            RotateTo(component);
            
            _enemyInDamageAreaBehaviour.Execute(_closestEnenmy);
            _attackTrigger.TriggerAttackStart();
            StartCooldownTimer();
            _closestEnenmy.Death += OnEnemyDeath;
        }

        private void RecalculateInCloseCombatClosestEnemy()
        {
            foreach (var enemy in _inCloseCombatAreaDamagables)
            {
                if(enemy is not MonoBehaviour component) continue;
                var distance = Vector3.Distance(GetHitterCartesianPos(), GetEnemyCartesianPos(component));
                if (distance > _distanceToClosestEnemy) continue;
                _distanceToClosestEnemy = distance;
                _closestEnenmy = enemy;
            }
        }

        private void OnEnemyDeath(IDamageable enemy)
        {
            if(enemy is not MonoBehaviour component) continue;
            ClosestEnemyDeath?.Invoke();
            if (component.TryGetComponent<IsoCharacterMover>(out var mover)) mover.MovementChanged -= OnTargetMoved;
            _closestEnenmy = null;
            _distanceToClosestEnemy = float.MaxValue;
            if (_inCloseCombatAreaDamagables.Count == 0) return;
            RecalculateInCloseCombatClosestEnemy();
            var newComponent = _closestEnenmy as MonoBehaviour;
            if(newComponent == null) return;
            if(newComponent.TryGetComponent<IsoCharacterMover>(out var newMover)) newMover.MovementChanged += OnTargetMoved;
            RotateTo(newComponent);
        }

        private void OnTargetMoved(object sender, IsoCharacterMover.MovementActionEventArgs movementActionEventArgs)
        {
            var component = _closestEnenmy as MonoBehaviour;
            if(component == null) return;
            if(movementActionEventArgs.Status == IsoCharacterMover.MovementStatus.Started) RotateTo(component);
        }

        private void RotateTo(Component target)
        {
            var direction = GetMovementDirectionTo(target);
            if(_currentDirection == direction) return;
            _currentDirection = direction;
            _ai.Rotate(_currentDirection);
        }

        private MovementDirection GetMovementDirectionTo(Component target)
        {
            var vectorDirection = GetVectorDirection(target);
            var direction = InputVectorToDirectionConverter.GetMovementDirectionFor(vectorDirection);
            return direction;
        }

        private Vector3 GetVectorDirection(Component target)
        {
            var enemyCartesianPos = GetEnemyCartesianPos(target);
            var hitterCartesianPos = GetHitterCartesianPos();

            var vectorDirection = (enemyCartesianPos - hitterCartesianPos).normalized;
            return vectorDirection;
        }

        private Vector3 GetHitterCartesianPos()
        {
            var hitterWorldPos = _ai.transform.position;
            var hitterVectorPos = new Vector3Iso(hitterWorldPos.x, hitterWorldPos.y, hitterWorldPos.z);
            var hitterCartesianPos = hitterVectorPos.ToCartesian();
            return hitterCartesianPos;
        }

        private static Vector3 GetEnemyCartesianPos(Component target)
        {
            var enemyWorldPosition = target.transform.position;
            var enemyIsoVectorPos = new Vector3Iso(enemyWorldPosition.x, enemyWorldPosition.y, enemyWorldPosition.z);
            var enemyCartesianPos = enemyIsoVectorPos.ToCartesian();
            return enemyCartesianPos;
        }

        private void OnTargetEnteredInSearch2DTriggerArea(Collider2D target)
        {
            if(!target.TryGetComponent<IDamager>(out var enemy)) return; 
            _enemyInSearchAreaBehaviour.Execute(enemy);
        }

        public void StopListeningAreasSignals()
        {
            StopListeningSearchTriggerSignals();
            _closeCombat2DTriggerArea.TargetEnteredInArea -= OnTargetEnteredInCloseCombat;
            _closeCombat2DTriggerArea.TargetExitFromArea -= OnTargetExitFromArea;
        }

        public void StopListeningSearchTriggerSignals()
        {
            _search2DTriggerArea.TargetEnteredInArea -= OnTargetEnteredInSearch2DTriggerArea;
        }
        
        
        private void StartCooldownTimer()
        {
            _attackCooldownTimer ??= TimersFabric.Create(Timer.TimerType.ScaledFrame, _attackCooldown);
            _attackCooldownTimer.TimerFinished += OnCooldownFinished;
            _attackCooldownTimer.Restart();
        }
        
        private void OnCooldownFinished()
        {
            if(_closestEnenmy == null) return;
            _enemyInDamageAreaBehaviour.Execute(_closestEnenmy);
            _attackTrigger.TriggerAttackStart();
            _attackCooldownTimer.Restart();
        }

        public void Dispose()
        {
            StopListeningAreasSignals();
            _enemyInDamageAreaBehaviour = null;
            _enemyInSearchAreaBehaviour = null;
            _search2DTriggerArea = null;
            Destroy(this);
            GC.SuppressFinalize(this);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
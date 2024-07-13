using System;
using Base.Input;
using Base.Math;
using Cysharp.Threading.Tasks;
using Source.BattleSystem;
using Source.Character.AI;
using Source.Character.AI.BattleAI;
using Source.Character.Minions_Manager;
using UnityEngine;

namespace Source.Formations
{
    public class InFormationPathfinder
    {
        private const float DelayBeforeReturningToFormationInSeconds = 30f;

        private MinionsTargetPositionCoordinator _targetPositionCoordinator;
        private Transform _inFormationPositionTransform;

        private Transform _targetTransform = null;
        
        private bool _listeningBattleAI = false;

        public PathfinderAI AI { get; }
        public BattleAI BattleAI { get; }

        public event Action<InFormationPathfinder> ReturningToFormation;

        public InFormationPathfinder(PathfinderAI pathfinderAI, Transform inFormationPositionTransform,
            MinionsTargetPositionCoordinator targetPositionCoordinator, BattleAI battleAI)
        {
            AI = pathfinderAI;
            BattleAI = battleAI;
            _inFormationPositionTransform = inFormationPositionTransform;
            _targetPositionCoordinator = targetPositionCoordinator;
            AI.ItemInManipulator += ReturnToCoordinator;
        }

        public void Dispatch()
        {
            AI.StartAnalyzingInformationSources();
            StartListeningBattleAI();
            StartListeningTargetChanging();
            BattleAI.StartListeningAreasSignals();
            AI.PathStarted += StopListeningTargetChanging;
            AI.PathCancelled += ReturnToFormationWithDelay;
        }

        private void StartListeningBattleAI()
        {
            _listeningBattleAI = true;
            BattleAI.ClosestEnemyDeath += OnEnemyDeath;
        }

        private void OnEnemyDeath()
        {
            StopListeningBattleAI();
            BattleAI.StopListeningAreasSignals();
            AI.StartFollowingPath(_inFormationPositionTransform.position);
            AI.PathCancelled += AddToFormation;
        }

        private void StopListeningBattleAI()
        {
            _listeningBattleAI = false;
            BattleAI.ClosestEnemyDeath -= OnEnemyDeath;
        }
        
        private void ReturnToCoordinator()
        {
            StopListeningBattleAI();
            AI.PathCancelled -= ReturnToFormationWithDelay;
            AI.StartFollowingPath(_targetPositionCoordinator.transform.position);
            AI.PathCancelled += ReturnToFormation;
        }

        private async void ReturnToFormationWithDelay()
        {
            if(_listeningBattleAI) return;
            await UniTask.Delay(TimeSpan.FromSeconds(DelayBeforeReturningToFormationInSeconds));
            StopListeningBattleAI();
            ReturnToFormation();
        }

        public void ReturnToFormation()
        {
            if(_listeningBattleAI) return;
            BattleAI.StopListeningAreasSignals();
            StopListeningBattleAI();
            AI.PathCancelled -= ReturnToFormationWithDelay;
            AI.PathCancelled -= ReturnToFormation;
            AI.StartFollowingPath(_inFormationPositionTransform.position);
            AI.PathCancelled += AddToFormation;
        }

        public void StartListeningTargetChanging()
        {
            _targetPositionCoordinator.TargetPositionChanged += StartFollowingTarget;
        }

        private void StartFollowingTarget(Vector3 target)
        {
            AI.StopFollowing();
            AI.StartFollowingPath(target);
        }

        private void StopListeningTargetChanging()
        {
            _targetPositionCoordinator.TargetPositionChanged -= StartFollowingTarget;
        }

        private void AddToFormation()
        {
            AI.PathStarted -= StopListeningTargetChanging;
            AI.PathCancelled -= AddToFormation;
            AI.Rotate(GetNormalizedCartesianDirectionTo(_inFormationPositionTransform.position));
            AI.StopAnalyzingInformationSources();
            StopListeningTargetChanging();
            StopListeningBattleAI();
            BattleAI.StopListeningAreasSignals();
            ReturningToFormation?.Invoke(this);
        }

        private MovementDirection GetNormalizedCartesianDirectionTo(Vector3 targetPosition)
        {
            var worldPosition = AI.transform.position;
            var worldDirection = targetPosition - new Vector3(worldPosition.x, worldPosition.y - 0.15f, worldPosition.z);
            var isometricDirection = new Vector3Iso(worldDirection.x, worldDirection.y, worldDirection.z);
            var cartesianDirection = isometricDirection.ToCartesian();
            return InputVectorToDirectionConverter.GetMovementDirectionFor(cartesianDirection.normalized);
        }
    }
}

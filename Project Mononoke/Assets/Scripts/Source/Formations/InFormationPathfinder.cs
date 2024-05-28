using System;
using Base.Input;
using Base.Math;
using Cysharp.Threading.Tasks;
using Source.Character.AI;
using Source.Character.Minions_Manager;
using UnityEngine;

namespace Source.Formations
{
    public class InFormationPathfinder
    {
        private const float DELAY_BEFORE_RETURNING_TO_FORMATION_IN_SECONDS = 30f; 
        private MinionsTargetPositionCoordinator _targetPositionCoordinator  = null;
        public PathfinderAI AI { get; }
        private Transform _inFormationPositionTransform { get; }

        public event Action<InFormationPathfinder> ReturningToFormation = null;

        public InFormationPathfinder(PathfinderAI pathfinderAI, Transform inFormationPositionTransform, MinionsTargetPositionCoordinator targetPositionCoordinator)
        {
            AI = pathfinderAI;
            AI.ItemInManipulator += ReturnToCoordinator;
            _inFormationPositionTransform = inFormationPositionTransform;
            _targetPositionCoordinator = targetPositionCoordinator;
        }

        private void ReturnToCoordinator()
        {
            AI.PathCancelled -= ReturnToFormationWithDelay;
            AI.StartFollowingPath(_targetPositionCoordinator.transform.position);
            AI.PathCancelled += ReturnToFormation;
        }
        
        private async void ReturnToFormationWithDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY_BEFORE_RETURNING_TO_FORMATION_IN_SECONDS));
            ReturnToFormation();
        }

        public void ReturnToFormation()
        {
            AI.PathCancelled -= ReturnToFormationWithDelay;
            AI.PathCancelled -= ReturnToFormation;
            AI.StartFollowingPath(_inFormationPositionTransform.position);
            AI.PathCancelled += AddToFormation;
        }
        
        public void StartListeningTargetChanging()
        {
            _targetPositionCoordinator.TargetPositionChanged += AI.StartFollowingPath;
        }

        private void StopListeningTargetChanging()
        {
            _targetPositionCoordinator.TargetPositionChanged -= AI.StartFollowingPath;
        }

        public void Dispatch()
        {
            AI.StartAnalyzingInformationSources();
            StartListeningTargetChanging();
            AI.PathStarted += StopListeningTargetChanging;
            AI.PathCancelled += ReturnToFormationWithDelay;
        }

        private void AddToFormation()
        {
            AI.PathStarted -= StopListeningTargetChanging;
            AI.PathCancelled -= AddToFormation;
            AI.Rotate(GetNormalizedCartesianDirectionTo(_inFormationPositionTransform.position));
            AI.StopAnalyzingInformationSources();
            StopListeningTargetChanging();
            ReturningToFormation?.Invoke(this);
        }
        
        private MovementDirection GetNormalizedCartesianDirectionTo(Vector3 targetPosition)
        {
            var worldPosition = AI.gameObject.transform.position;
            var worldDirection = targetPosition - new Vector3(worldPosition.x, worldPosition.y-0.15f, worldPosition.z);
            var isometricDirection = new Vector3Iso(worldDirection.x, worldDirection.y, worldDirection.z);
            var cartesianDirection = isometricDirection.ToCartesian();
            return InputVectorToDirectionConverter.GetMovementDirectionFor(cartesianDirection.normalized);
        }
    }
}
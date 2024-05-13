using System;
using Cysharp.Threading.Tasks;
using Source.Character.AI;
using UnityEngine;

namespace Source.Formations
{
    public class InFormationPathfinder
    {
        private const float DELAY_BEFORE_RETURNING_TO_FORMATION_IN_SECONDS = 1f; 
        public PathfinderAI AI { get; }
        private Transform _inFormationPositionTransform { get; }

        public event Action<InFormationPathfinder> ReturningToFormation = null;

        public InFormationPathfinder(PathfinderAI pathfinderAI, Transform inFormationPositionTransform)
        {
            AI = pathfinderAI;
            _inFormationPositionTransform = inFormationPositionTransform;
        }

        private async void ReturnToFormationWithDelay()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DELAY_BEFORE_RETURNING_TO_FORMATION_IN_SECONDS));
            ReturnToFormation();
        }

        public void ReturnToFormation()
        {
            AI.PathCancelled -= ReturnToFormationWithDelay;
            AI.StartFollowingPath(_inFormationPositionTransform.position);
            AI.PathCancelled += AddToFormation;
        }

        private void StopListeningTargetChanging()
        {
            AI.StopListeningTargetChanging();
        }

        public void Dispatch()
        {
            AI.StartAnalyzingInformationSources();
            AI.PathStarted += StopListeningTargetChanging;
            AI.PathCancelled += ReturnToFormationWithDelay;
        }

        private void AddToFormation()
        {
            AI.PathStarted -= StopListeningTargetChanging;
            AI.PathCancelled -= AddToFormation;
            AI.StopAnalyzingInformationSources();
            ReturningToFormation?.Invoke(this);
        }
    }
}
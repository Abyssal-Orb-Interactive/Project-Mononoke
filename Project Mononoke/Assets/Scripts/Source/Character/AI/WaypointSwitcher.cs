using System;
using System.Collections.Generic;
using Base.Counters;
using Base.Math.TypesWrappers;
using UnityEngine;

namespace Source.Character.AI
{
    public class WaypointSwitcher
    {
        private readonly IReadOnlyList<Vector3> _waypointsPositions = null;
        private readonly Counter<IntWrapper> _waypointIndexCounter = null;
        private readonly Transform _currentPosition = null;
        private readonly float _pathNodeProximityThreshold = 3f;

        public event Action LastWaypointReached = null;
        public event Action<Vector3> WaypointChanged = null; 


        public WaypointSwitcher(IReadOnlyList<Vector3> waypointsPositions, float pathNodeProximityThreshold,
            Transform currentPosition)
        {
            _waypointsPositions = waypointsPositions;
            _waypointIndexCounter = new Counter<IntWrapper>(new IntWrapper(0), new IntWrapper(_waypointsPositions.Count - 1), () => new IntWrapper(1));
            _pathNodeProximityThreshold = pathNodeProximityThreshold;
            _currentPosition = currentPosition;
            _waypointIndexCounter.TargetReached += OnLastWaypointReached;
            _waypointIndexCounter.ValueChanged += OnWaypointChanged;
        }

        private void OnWaypointChanged()
        {
            WaypointChanged?.Invoke(_waypointsPositions[_waypointIndexCounter.CurrentValue.Value]);
        }

        private void OnLastWaypointReached()
        {
            LastWaypointReached?.Invoke();
        }

        public void CheckIfReachedCurrentWayPointAndSwitchToNextOneIfNecessary()
        {
            if (IsThresholdContains(_currentPosition.position)) _waypointIndexCounter.CalculateNextValue();
        }

        private bool IsThresholdContains(Vector3 position)
        {
            return CalculateDistanceBetween(position, _waypointsPositions[_waypointIndexCounter.CurrentValue.Value]) <
                   _pathNodeProximityThreshold;
        }

        private float CalculateDistanceBetween(Vector3 currentPosition, Vector3 targetPosition)
        {
            return Vector3.Distance(currentPosition, targetPosition);
        }
    }
}
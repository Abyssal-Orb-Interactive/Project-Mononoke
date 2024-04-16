using System;
using Base.Input.Actions;
using Base.Math;
using Pathfinding;
using UnityEngine;

namespace Source.Character.AI
{
    [RequireComponent(typeof(Seeker))]
    public class PathfinderAI : MonoBehaviour, IInputSource
    {
        [SerializeField] private Seeker _pathBuilder = null;
        private PathWaypointsPositionsSource _waypointsPositionsSource = null;
        private WaypointSwitcher _waypointSwitcher = null;
        private Vector3 _cartesianMovementDirection = Vector3.zero;
        private bool _isPatCancelled = false;

        public event Action<MovementInputEventArgs> MovementDesired, MovementCancelled;
        private void OnValidate()
        {
            _pathBuilder ??= GetComponent<Seeker>();
        }

        public async void StartFollowing(Vector3 targetPosition)
        {
            _waypointsPositionsSource = new PathWaypointsPositionsSource(_pathBuilder);
            var waypointsPositions = await _waypointsPositionsSource.GetWaypointsFor(transform.position, targetPosition);
            _waypointSwitcher = new WaypointSwitcher(waypointsPositions, 3f, transform);
            _waypointSwitcher.WaypointChanged += CalculateNormalizedCartesianDirectionTo;
            _waypointSwitcher.LastWaypointReached += OnTargetReached;
            _isPatCancelled = false;

            while (!_isPatCancelled)
            {
                _waypointSwitcher.CheckIfReachedCurrentWayPointAndSwitchToNextOneIfNecessary();
                MovementDesired?.Invoke(new MovementInputEventArgs(_cartesianMovementDirection));
            }
        }

        private void CalculateNormalizedCartesianDirectionTo(Vector3 currentWaypointPosition)
        {
            var worldDirection = currentWaypointPosition - transform.position;
            var isometricDirection = new Vector3Iso(worldDirection.x, worldDirection.y, worldDirection.z);
            var cartesianDirection = isometricDirection.ToCartesian();
            _cartesianMovementDirection = cartesianDirection.normalized;
        }
        
        private void OnTargetReached()
        {
            _isPatCancelled = true;
            MovementCancelled?.Invoke(new MovementInputEventArgs(Vector2.zero));
        }

        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void SubscribeToMovementInputStarts(Action<MovementInputEventArgs> contextAction)
        {
            MovementDesired += contextAction;
        }

        public void SubscribeToMovementInputEnds(Action<MovementInputEventArgs> contextAction)
        {
            MovementCancelled += contextAction;
        }

        public void UnsubscribeToMovementInputStarts(Action<MovementInputEventArgs> contextAction)
        {
            MovementDesired -= contextAction;
        }

        public void UnsubscribeToMovementInputEnds(Action<MovementInputEventArgs> contextAction)
        {
            MovementCancelled -= contextAction;
        }

        public void Enable()
        {
            throw new NotImplementedException();
        }

        public void Disable()
        {
            throw new NotImplementedException();
        }
    }
}
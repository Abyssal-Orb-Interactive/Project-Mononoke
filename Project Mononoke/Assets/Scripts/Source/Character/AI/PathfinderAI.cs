using System;
using System.Threading;
using Base.Input.Actions;
using Base.Math;
using Cysharp.Threading.Tasks;
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
        private bool _isPathCancelled = false;
        private CancellationTokenSource _cancellationTokenSource = null;

        public event Action<MovementInputEventArgs> MovementDesired, MovementCancelled;
        private void OnValidate()
        {
            _pathBuilder ??= GetComponent<Seeker>();
        }

        public async void StartFollowing(Vector3 targetPosition)
        {
            StopFollowing();
            _waypointsPositionsSource = new PathWaypointsPositionsSource(_pathBuilder);
            var waypointsPositions = await _waypointsPositionsSource.GetWaypointsFor(transform.position, targetPosition);
            _waypointSwitcher = new WaypointSwitcher(waypointsPositions, 3f, transform);
            _waypointSwitcher.WaypointChanged += CalculateNormalizedCartesianDirectionTo;
            _waypointSwitcher.LastWaypointReached += OnTargetReached;
            
            
            _cancellationTokenSource = new CancellationTokenSource();
            _isPathCancelled = false;
            FollowPathAsync().Forget();
        }

        public void StopFollowing()
        {
            _cancellationTokenSource?.Cancel();
            //z_cancellationTokenSource?.Dispose();
            //_cancellationTokenSource = null;
            _isPathCancelled = true;
            MovementCancelled?.Invoke(new MovementInputEventArgs(Vector2.zero));
        }
        
        private async UniTaskVoid FollowPathAsync()
        {
            
            while (!_cancellationTokenSource.Token.IsCancellationRequested && !_isPathCancelled)
            {
                _waypointSwitcher.CheckIfReachedCurrentWayPointAndSwitchToNextOneIfNecessary();

                MovementDesired?.Invoke(new MovementInputEventArgs(_cartesianMovementDirection));
                await UniTask.Delay(TimeSpan.FromSeconds(0.1), cancellationToken: _cancellationTokenSource.Token);
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
            StopFollowing();
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
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            throw new NotImplementedException();
        }
    }
}
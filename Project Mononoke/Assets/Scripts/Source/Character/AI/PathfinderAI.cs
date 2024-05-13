using System;
using System.Threading;
using Base.Input.Actions;
using Base.Math;
using Cysharp.Threading.Tasks;
using Pathfinding;
using Source.BuildingModule;
using Source.Character.Minions_Manager;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using VContainer;

namespace Source.Character.AI
{
    [RequireComponent(typeof(Seeker))]
    public class PathfinderAI : MonoBehaviour, IInputSource
    {
        [SerializeField] private Seeker _pathBuilder = null;
        [SerializeField] private MinionsTargetPositionCoordinator _minionsTargetPositionCoordinator = null;
        private CollidersHolder _collidersHolder = null;
        private PathWaypointsPositionsSource _waypointsPositionsSource = null;
        private WaypointSwitcher _waypointSwitcher = null;
        private Vector3 _cartesianMovementDirection = Vector3.zero;
        private CancellationTokenSource _cancellationTokenSource = null;
        private bool _pathCancelled = false;
        private PickUpper _pickUpper = null;

        public event Action<MovementInputEventArgs> MovementDesired, MovementCancelled;
        public event Action PathStarted, PathCancelled;
        private void OnValidate()
        {
            _pathBuilder ??= GetComponent<Seeker>();
        }

        [Inject]
        public void Initialize(MinionsTargetPositionCoordinator minionsTargetPositionCoordinator, CollidersHolder collidersHolder, PickUpper pickUpper)
        {
            _minionsTargetPositionCoordinator = minionsTargetPositionCoordinator;
            _pickUpper = pickUpper;
            _collidersHolder = collidersHolder;
            StartAnalyzingInformationSources();
        }

        public void StartListeningColliders()
        {
            _collidersHolder.SomethingInCollider += StopFollowingAndInteract;
        }

        public void StartListeningTargetChanging()
        {
            _minionsTargetPositionCoordinator.TargetPositionChanged += StartFollowingPath;
        }

        public void StopAnalyzingInformationSources()
        {
            StopListeningColliders();
            StopListeningTargetChanging();
        }

        public void StopListeningTargetChanging()
        {
            _minionsTargetPositionCoordinator.TargetPositionChanged -= StartFollowingPath;
        }

        public void StopListeningColliders()
        {
            _collidersHolder.SomethingInCollider -= StopFollowingAndInteract;
        }

        public void StartAnalyzingInformationSources()
        {
            StartListeningColliders();
            StartListeningTargetChanging();
        }

        private void StopFollowingAndInteract(object something)
        {
           StopFollowing();
           
           switch (something)
           {
               case Item item:
                   _pickUpper.TryTakeItemFromInventoryWithManipulator(item.Data.ID);
                   break;
               case Building building:
                   building.StartInteractiveAction(_pickUpper);
                   break;
               default:
                   return;
           }
           
           StartFollowingPath(_minionsTargetPositionCoordinator.transform.position);
        }

        public async void StartFollowingPath(Vector3 targetPosition)
        {
            StopFollowing();
            PathStarted?.Invoke();
            _waypointsPositionsSource = new PathWaypointsPositionsSource(_pathBuilder);
            var position = transform.position;
            var waypointsPositions = await _waypointsPositionsSource.GetWaypointsFor(new Vector3(position.x, position.y - 0.15f, position.z), targetPosition);
            _waypointSwitcher = new WaypointSwitcher(waypointsPositions, 0.25f, transform);
            _waypointSwitcher.WaypointChanged += CalculateNormalizedCartesianDirectionTo;
            _waypointSwitcher.LastWaypointReached += OnTargetReached;
            
            
            _cancellationTokenSource = new CancellationTokenSource();
            _pathCancelled = false;
            FollowPathAsync().Forget();
            PathCancelled?.Invoke();
        }

        public void StopFollowing()
        {
            _pathCancelled = true;
            _cancellationTokenSource?.Cancel();
            MovementCancelled?.Invoke(new MovementInputEventArgs(Vector2.zero));
        }
        
        private async UniTaskVoid FollowPathAsync()
        {
            
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                CalculateNormalizedCartesianDirectionTo(_waypointSwitcher.CurrentWaypointPosition);
                _waypointSwitcher.CheckIfReachedCurrentWayPointAndSwitchToNextOneIfNecessary();
                if(!_pathCancelled) MovementDesired?.Invoke(new MovementInputEventArgs(_cartesianMovementDirection));
                await UniTask.Delay(TimeSpan.FromSeconds(0.005f), cancellationToken: _cancellationTokenSource.Token);
            }

        }

        public async void StartFollowing(Vector3 position, float positionProximityThreshold)
        {
            StopFollowing();
            _cancellationTokenSource = new CancellationTokenSource();
            _pathCancelled = false;
            
            FollowAsync(position, positionProximityThreshold).Forget();
        }

        private async UniTaskVoid FollowAsync(Vector3 position, float positionProximityThreshold)
        {
            CalculateNormalizedCartesianDirectionTo(position);
            if (Vector3.Distance(transform.position, position) < positionProximityThreshold) StopFollowing();
            if(!_pathCancelled) MovementDesired?.Invoke(new MovementInputEventArgs(_cartesianMovementDirection));
            await UniTask.Delay(TimeSpan.FromSeconds(0.005f), cancellationToken: _cancellationTokenSource.Token);
        }

        private void CalculateNormalizedCartesianDirectionTo(Vector3 currentWaypointPosition)
        {
            var pos = transform.position;
            var worldDirection = currentWaypointPosition - new Vector3(pos.x, pos.y-0.15f, pos.z);
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
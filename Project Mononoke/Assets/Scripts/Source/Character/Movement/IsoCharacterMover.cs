using System;
using Base.Input;
using Base.Math;
using UnityEngine;
using InputHandler = Base.Input.InputHandler;
using VContainer;
using Base.Grid;

namespace Source.Character.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class IsoCharacterMover : MonoBehaviour, IDisposable, IPositionSource
    {
        public enum MovementStatus
        {
            Started = 0,
            Ended = 1
        }

        public struct MovementActionEventArgs
        {
            public MovementStatus Status { get; }
            public MovementDirection Facing { get; }

            public MovementActionEventArgs(MovementStatus status, MovementDirection facing)
            {
                Status = status;
                Facing = facing;
            }
        }

        public EventHandler<MovementActionEventArgs> MovementChanged;
        
        [SerializeField] private float _speed = 10f;

        private Rigidbody2D _rigidbody = null;
        private InputHandler _inputHandler = null;
        private MovementDirection _moveDirection;
        private GroundGrid _grid = null;
        private bool _movementDesired = false;

        private void OnValidate()
        {
            GetRigidbody();
        }

        private void GetRigidbody()
        {
            _rigidbody ??= GetComponent<Rigidbody2D>();
        }

        [Inject] public void Initialize(GroundGrid grid, InputHandler inputHandler)
        {
            _grid = grid;
            _inputHandler = inputHandler;
            StartInputHandling();
        }

        public Vector3 GetCurrentLogicalPosition()
        {
            var pos = GetCurrentPosition();
            return new Vector3(pos.x, pos.y - 0.3f, pos.z);
        }
        
        private Vector3 GetCurrentPosition()
        {
            GetRigidbody();
            return _rigidbody.position;
        }

        private void Update()
        {
            MoveTo(_moveDirection);
        }

        private void MoveTo(MovementDirection direction)
        {
            if (_movementDesired == false)
            {
                MovementChanged?.Invoke(this, new MovementActionEventArgs(MovementStatus.Ended, _moveDirection));
                return;
            } 
            var targetPosition = CalculateInGridTargetPosition(direction, out var inGridPosition);
            if (!_grid.IsCellPassableAt(inGridPosition))
            {
                MovementChanged?.Invoke(this, new MovementActionEventArgs(MovementStatus.Ended, _moveDirection));
                return;
            }
            _rigidbody.MovePosition(targetPosition);
            MovementChanged?.Invoke(this, new MovementActionEventArgs(MovementStatus.Started, _moveDirection));
        }

        private Vector3 CalculateInGridTargetPosition(MovementDirection direction, out Vector3Int inGridPosition)
        {
            var offset = DirectionToVector3IsoConverter.ToVector(direction) * (_speed * Time.deltaTime); 
            var position = GetCurrentPosition();
            var targetPosition = position + offset;
            var legsTargetPosition = new Vector3(targetPosition.x, targetPosition.y - 0.3f, targetPosition.z);
            inGridPosition = _grid.WorldToGrid(legsTargetPosition);
            return targetPosition;
        }

        public PositionData GetPositionData()
        {
            var worldPosition = GetCurrentLogicalPosition();
            return new PositionData(_moveDirection, worldPosition);
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            _moveDirection = (MovementDirection)args.ActionData;
            _movementDesired = args.Status == InputHandler.InputActionEventArgs.ActionStatus.Started;
        }

        private void StartInputHandling()
        {
            if(_inputHandler == null) return;
            _inputHandler.StartInputHandling();
            _inputHandler.AddInputChangedHandler(OnMovementInputChange);
        }
        
        private void StopInputHandling()
        {
            if(_inputHandler == null) return;
            _inputHandler.StopInputHandling();
            _inputHandler.RemoveInputChangedHandler(OnMovementInputChange);
        }
        
        private void OnEnable()
        {
            if(_inputHandler == null) return;
            StartInputHandling();
        }

        private void OnDisable()
        {
            if(_inputHandler == null) return;
            StopInputHandling();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            StopInputHandling();
            _inputHandler = null;
        }
    }
}
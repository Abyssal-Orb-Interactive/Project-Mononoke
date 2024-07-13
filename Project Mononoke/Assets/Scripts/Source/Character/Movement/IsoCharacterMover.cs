using System;
using Base.Input;
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
        [SerializeField] private float _spriteToLegsOffset = 0.3f;
        [SerializeField] private Rigidbody2D _rigidbody = null;
        
        private InputHandler _inputHandler = null;
        private MovementDirection _moveDirection;
        private GroundGrid _grid = null;
        private bool _movementDesired = false;

        [Inject] public void Initialize(GroundGrid grid, InputHandler inputHandler)
        {
            _grid = grid;
            _inputHandler = inputHandler;
            _rigidbody = GetComponent<Rigidbody2D>();
            
            StartInputHandling();
        }

        private Vector3 GetCurrentPosition()
        {
            if (_rigidbody == null) _rigidbody.GetComponent<Rigidbody2D>();
            return _rigidbody.position;
        }

        public void UpdatePosition()
        {
            MoveTo(_moveDirection);
        }

        private void Rotate(MovementDirection direction)
        { 
            if(_moveDirection == direction) return;
            _moveDirection = direction;
            MovementChanged?.Invoke(this, new MovementActionEventArgs(MovementStatus.Ended, _moveDirection));
        }

        private void MoveTo(MovementDirection direction)
        {
            if (_movementDesired == false)
            {
                Rotate(direction);
                return;
            }
            var targetPosition = CalculateInGridTargetPosition(direction, out var inGridPosition);
            if (!_grid.IsCellPassableAt(inGridPosition))
            {
                Rotate(direction);
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
            var legsTargetPosition = new Vector3(targetPosition.x, targetPosition.y - _spriteToLegsOffset, targetPosition.z);
            inGridPosition = _grid.WorldToGrid(legsTargetPosition);
            return targetPosition;
        }

        public PositionData GetPositionData()
        {
            var worldPosition = GetCurrentPosition();
            return new PositionData(_moveDirection, worldPosition);
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            Rotate((MovementDirection)args.ActionData);
            _movementDesired = args.Status == InputHandler.InputActionEventArgs.ActionStatus.Started;
            MovementChanged?.Invoke(this,
                _movementDesired == false
                    ? new MovementActionEventArgs(MovementStatus.Ended, _moveDirection)
                    : new MovementActionEventArgs(MovementStatus.Started, _moveDirection));
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

        private void OnDrawGizmos()
        {
            var position = GetCurrentPosition();
            var legsPos = new Vector3(position.x, position.y - _spriteToLegsOffset, position.z);
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawCube(legsPos, new Vector3(0.1f, 0.1f, 0.1f));
        }
    }
}
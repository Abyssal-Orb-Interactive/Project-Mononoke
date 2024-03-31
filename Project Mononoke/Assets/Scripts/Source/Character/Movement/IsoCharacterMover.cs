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
            _rigidbody ??= GetComponent<Rigidbody2D>();
            
        }

        [Inject] private void Initialize(GroundGrid grid, InputHandler inputHandler)
        {
            _grid = grid;
            _inputHandler = inputHandler;
            StartInputHandling();
        }

        public Vector3 GetCurrentPosition()
        {
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
                MovementChanged.Invoke(this, new MovementActionEventArgs(MovementStatus.Ended, _moveDirection));
                return;
            } 
            var offset =  DirectionToVector3IsoConverter.ToVector(direction) * (_speed * Time.deltaTime);
            var isoOffset = new Vector2Iso(offset);
            var position = _rigidbody.position;
            var targetPosition = new Vector3(position.x + isoOffset.X, position.y + isoOffset.Y, 0);
            var inGridPosition = _grid.WorldToGrid(targetPosition);
            if (!_grid.IsCellPassableAt(inGridPosition))
            {
                MovementChanged.Invoke(this, new MovementActionEventArgs(MovementStatus.Ended, _moveDirection));
                return;
            }

            _rigidbody.MovePosition(targetPosition);
            MovementChanged.Invoke(this, new MovementActionEventArgs(MovementStatus.Started, _moveDirection));
        }

        public PositionData GetPosition()
        {
            var worldPosition = transform.position;
            var isoPosition = new Vector3Iso(worldPosition.x, worldPosition.y, worldPosition.z);
            var cartesianPosition = isoPosition.ToCartesian();
            return new PositionData(_moveDirection, cartesianPosition);
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
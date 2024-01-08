using System;
using Base.Input;
using Base.Math;
using UnityEngine;
using InputHandler = Base.Input.InputHandler;
using VContainer;

namespace Source.Character.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class IsoCharacterMover : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _speed = 10f;

        private Rigidbody2D _rigidbody = null;
        private InputHandler _inputHandler = null;
        private MovementDirection _moveDirection = MovementDirection.Stay;
        private GridAnalyzer _gridAnalyzer = null;

        private void OnValidate()
        {
            _rigidbody ??= GetComponent<Rigidbody2D>();
            
        }

        [Inject] private void Initialize(GridAnalyzer gridAnalyzer, InputHandler inputHandler)
        {
            _gridAnalyzer = gridAnalyzer;
            _inputHandler = inputHandler;
            StartInputHandling();
        }

        private void FixedUpdate()
        {
            MoveTo(_moveDirection);
        }

        private void MoveTo(MovementDirection direction)
        {
            if(direction == MovementDirection.Stay) return; 
            var offset =  DirectionToVector3Converter.ToVector(direction) * (_speed * Time.deltaTime);
            var isoOffset = new Vector2Iso(offset);
            var position = _rigidbody.position;
            var targetPosition = new Vector3(position.x + isoOffset.X, position.y + isoOffset.Y, 0);
            if(!_gridAnalyzer.IsNextCellMovable(transform.position, _moveDirection)) 
            {
                Vector3Iso isoCellSize = _gridAnalyzer.GetNextCellSizes(transform.position, direction);
                if (targetPosition.x - position.x > isoCellSize.X / 2 || targetPosition.y - position.y > isoCellSize.Y / 2)
                {
                    return;
                }
            }
            _rigidbody.MovePosition(targetPosition);
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            _moveDirection = (MovementDirection)args.ActionData;
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
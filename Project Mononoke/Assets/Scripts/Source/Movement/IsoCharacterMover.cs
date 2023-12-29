using System;
using Base.Input;
using Base.Math;
using UnityEngine;
using InputHandler = Base.Input.InputHandler;

namespace Source.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class IsoCharacterMover : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _speed = 10f;

        private Rigidbody2D _rigidbody = null;
        private InputHandler _inputHandler = null;
        private MovementDirection _moveDirection = MovementDirection.Stay;

        private void OnValidate()
        {
            _rigidbody ??= GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
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
            var targetPosition = new Vector2(position.x + isoOffset.X, position.y + isoOffset.Y);
            
            _rigidbody.MovePosition(targetPosition);
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            _moveDirection = (MovementDirection)args.ActionData;
        }

        private void StartInputHandling()
        {
            _inputHandler ??= new InputHandler(new TestActions());
            _inputHandler.StartInputHandling();
            _inputHandler.AddInputChangedHandler(OnMovementInputChange);
        }
        
        private void StopInputHandling()
        {
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
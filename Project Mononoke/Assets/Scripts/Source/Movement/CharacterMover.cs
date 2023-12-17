using System;
using UnityEngine;
using InputHandler = Base.Input.InputHandler;

namespace Source.Movement
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CharacterMover : MonoBehaviour, IDisposable
    {
        [SerializeField] private float _speed = 10f;

        private Rigidbody2D _rigidbody = null;
        private InputHandler _inputHandler = null;
        private Vector2 _moveDirection = Vector2.zero;

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
            Move(_moveDirection);
        }

        private void Move(Vector3 direction)
        {
            if(direction == Vector3.zero) return; 
            var offset = direction * (_speed * Time.deltaTime);
            
            _rigidbody.MovePosition(_rigidbody.position + (Vector2)offset);
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            _moveDirection = (Vector2) args.ActionData;
           
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
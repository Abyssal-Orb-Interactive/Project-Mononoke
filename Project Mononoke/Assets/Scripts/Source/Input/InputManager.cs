using System;
using Base.Utils;
using UnityEngine;

namespace Source.Input
{
    public delegate void InputAction();
    public partial class InputManager : MonoBehaviour, IDisposable
    {
        private TestActions _input = null;
        private Vector2 _movementVectorInIsometric = Vector2.zero;

        private EventHandler<OnMovementInputChangedEventArgs> _onMovementInputChanged;
        
        public void Initialize()
        {
            _input = new TestActions();
            StartInputHandling();
        }

        public void AddMovementInputChangedHandler(EventHandler<OnMovementInputChangedEventArgs> handler)
        {
            _onMovementInputChanged += handler;
        }
        
        public void RemoveMovementInputChangedHandler(EventHandler<OnMovementInputChangedEventArgs> handler)
        {
            _onMovementInputChanged -= handler;
        }
        
        private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext movementDirection)
        {
            var movementVectorInCartesian = movementDirection.ReadValue<Vector2>();
            _movementVectorInIsometric = VectorUtils.ConvertFromCartesianToIsometric(movementVectorInCartesian);
            _onMovementInputChanged?.Invoke(this, new OnMovementInputChangedEventArgs(_movementVectorInIsometric));
        }
        
        private void OnMovementCancelled(UnityEngine.InputSystem.InputAction.CallbackContext movementDirection)
        { 
            _movementVectorInIsometric = Vector2.zero;
            _onMovementInputChanged?.Invoke(this, new OnMovementInputChangedEventArgs(_movementVectorInIsometric));
        }

        private void StartInputHandling()
        {
            if (_input == null) return;
            _input.Enable();
            _input.PlayerActions.Movement.performed += OnMovementPerformed;
            _input.PlayerActions.Movement.canceled += OnMovementCancelled;
        }

        private void StopInputHandling()
        {
            if (_input == null) return;
            _input.Disable();
            _input.PlayerActions.Movement.performed -= OnMovementPerformed;
            _input.PlayerActions.Movement.canceled -= OnMovementCancelled;
        }
        
        private void OnEnable()
        {
            StartInputHandling();
        }

        private void OnDisable()
        {
            StopInputHandling();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            StopInputHandling();
            _input.Dispose();
            _onMovementInputChanged = null;
            _input = null;
            GC.SuppressFinalize(this);
        }
    }
}
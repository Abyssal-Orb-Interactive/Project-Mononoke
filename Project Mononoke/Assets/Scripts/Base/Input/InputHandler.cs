using System;
using Base.UnityExtensions;
using UnityEngine;

namespace Base.Input
{
    public partial class InputHandler : IDisposable
    {
        private TestActions _input = null;
        private Vector2 _movementVector = Vector2.zero;

        private EventHandler<InputActionEventArgs> _onInputChangedHandlers;

        private InputHandler(){}

        public InputHandler(TestActions inputSource)
        {
            _input = inputSource;
        }

        public void AddInputChangedHandler(EventHandler<InputActionEventArgs> handler)
        {
            _onInputChangedHandlers += handler;
        }
        
        public void RemoveInputChangedHandler(EventHandler<InputActionEventArgs> handler)
        {
            _onInputChangedHandlers -= handler;
        }
        
        private void OnMovementPerformed(UnityEngine.InputSystem.InputAction.CallbackContext movementDirection)
        {
            var cartesianNormalizedMovementVector =  movementDirection.ReadValue<Vector2>().normalized;
            _movementVector = cartesianNormalizedMovementVector.ToIsometric();
            _onInputChangedHandlers?.Invoke(this, new InputActionEventArgs(InputActionEventArgs.ActionType.Movement, _movementVector));
        }
        
        private void OnMovementCancelled(UnityEngine.InputSystem.InputAction.CallbackContext movementDirection)
        { 
            _movementVector = Vector2.zero;
            _onInputChangedHandlers?.Invoke(this, new InputActionEventArgs(InputActionEventArgs.ActionType.Movement, _movementVector));
        }

        public void StartInputHandling()
        {
            if (_input == null) return;
            _input.Enable();
            _input.PlayerActions.Movement.performed += OnMovementPerformed;
            _input.PlayerActions.Movement.canceled += OnMovementCancelled;
        }

        public void StopInputHandling()
        {
            if (_input == null) return;
            _input.Disable();
            _input.PlayerActions.Movement.performed -= OnMovementPerformed;
            _input.PlayerActions.Movement.canceled -= OnMovementCancelled;
        }

        public void Dispose()
        {
            StopInputHandling();
            _input.Dispose();
            _onInputChangedHandlers = null;
            _input = null;
            GC.SuppressFinalize(this);
        }
    }
}
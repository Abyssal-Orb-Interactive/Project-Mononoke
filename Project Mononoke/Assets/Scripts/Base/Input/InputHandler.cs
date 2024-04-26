    using System;
    using Base.Input.Actions;
    using UnityEngine;

    namespace Base.Input
    {
        public partial class InputHandler : IDisposable
        {
            private IInputSource _input = null;
            private MovementDirection _movementDirection;

            private EventHandler<InputActionEventArgs> _onInputChangedHandlers;

            private InputHandler(){}

            public InputHandler(IInputSource inputSource)
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
            
            private void OnMovementStarted(MovementInputEventArgs args)
            {
                _movementDirection = InputVectorToDirectionConverter.GetMovementDirectionFor(args.Value);
                _onInputChangedHandlers?.Invoke(this, new InputActionEventArgs(InputActionEventArgs.ActionType.Movement, _movementDirection, InputActionEventArgs.ActionStatus.Started));
            }
            
            private void OnMovementCancelled(MovementInputEventArgs args)
            {
                _onInputChangedHandlers?.Invoke(this, new InputActionEventArgs(InputActionEventArgs.ActionType.Movement, _movementDirection, InputActionEventArgs.ActionStatus.Ended));
            }

            public void StartInputHandling()
            {
                if (_input == null) return;
                _input.Enable();
                _input.SubscribeToMovementInputStarts(OnMovementStarted);
                _input.SubscribeToMovementInputEnds(OnMovementCancelled);
            }

            public void StopInputHandling()
            {
                if (_input == null) return;
                _input.Disable();
                _input.UnsubscribeToMovementInputStarts(OnMovementStarted);
                _input.UnsubscribeToMovementInputEnds(OnMovementCancelled);
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
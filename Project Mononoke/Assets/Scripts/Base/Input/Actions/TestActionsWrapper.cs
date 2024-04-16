using System;
using UnityEngine;

namespace Base.Input.Actions
{
    public class TestActionsWrapper : IInputSource
    {
        private readonly TestActions _actions = null;

        public TestActionsWrapper(TestActions actions)
        {
            _actions = actions;
        }


        public void SubscribeToMovementInputStarts(Action<MovementInputEventArgs> contextAction)
        {
            _actions.PlayerActions.Movement.performed += ctx => contextAction?.Invoke(new MovementInputEventArgs(ctx.ReadValue<Vector2>()));
        }
        

        public void SubscribeToMovementInputEnds(Action<MovementInputEventArgs> contextAction)
        {
            _actions.PlayerActions.Movement.canceled += ctx => contextAction?.Invoke(new MovementInputEventArgs(ctx.ReadValue<Vector2>()));
        }

        public void UnsubscribeToMovementInputStarts(Action<MovementInputEventArgs> contextAction)
        {
            _actions.PlayerActions.Movement.performed -= ctx => contextAction?.Invoke(new MovementInputEventArgs(ctx.ReadValue<Vector2>()));
        }

        public void UnsubscribeToMovementInputEnds(Action<MovementInputEventArgs> contextAction)
        {
            _actions.PlayerActions.Movement.canceled -= ctx => contextAction?.Invoke(new MovementInputEventArgs(ctx.ReadValue<Vector2>()));
        }

        public void Dispose()
        {
            _actions.Dispose();
        }

        public void Enable()
        {
            _actions.Enable();
        }

        public void Disable()
        {
            _actions.Disable();
        }
    }
}
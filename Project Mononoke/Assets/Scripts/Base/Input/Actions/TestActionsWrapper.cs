using System;
using UnityEngine.InputSystem;

namespace Base.Input.Actions
{
    public class TestActionsWrapper : IInputSource
    {
        private readonly TestActions _actions = null;

        public TestActionsWrapper(TestActions actions)
        {
            _actions = actions;
        }


        public void SubscribeToMovementInputStarts(Action<InputAction.CallbackContext> contextAction)
        {
            _actions.PlayerActions.Movement.performed += contextAction;
        }
        

        public void SubscribeToMovementInputEnds(Action<InputAction.CallbackContext> contextAction)
        {
            _actions.PlayerActions.Movement.canceled += contextAction;
        }

        public void UnsubscribeToMovementInputStarts(Action<InputAction.CallbackContext> contextAction)
        {
            _actions.PlayerActions.Movement.performed -= contextAction;
        }

        public void UnsubscribeToMovementInputEnds(Action<InputAction.CallbackContext> contextAction)
        {
            _actions.PlayerActions.Movement.canceled -= contextAction;
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
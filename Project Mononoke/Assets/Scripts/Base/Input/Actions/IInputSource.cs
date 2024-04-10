using System;
using UnityEngine.InputSystem;

namespace Base.Input.Actions
{
    public interface IInputSource : IDisposable
    {
        public void SubscribeToMovementInputStarts(Action<InputAction.CallbackContext> contextAction);
        public void SubscribeToMovementInputEnds(Action<InputAction.CallbackContext> contextAction);
        public void UnsubscribeToMovementInputStarts(Action<InputAction.CallbackContext> contextAction);
        public void UnsubscribeToMovementInputEnds(Action<InputAction.CallbackContext> contextAction);
        public void Enable();
        public void Disable();
    }
}
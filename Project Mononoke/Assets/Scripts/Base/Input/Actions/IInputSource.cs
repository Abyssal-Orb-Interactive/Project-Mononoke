using System;
using UnityEngine;

namespace Base.Input.Actions
{
    public class MovementInputEventArgs : EventArgs
    {
        public Vector2 Value { get; }

        public MovementInputEventArgs(Vector2 value)
        {
            Value = value;
        }
    }
    
    public interface IInputSource : IDisposable
    {
        public void SubscribeToMovementInputStarts(Action<MovementInputEventArgs> contextAction);
        public void SubscribeToMovementInputEnds(Action<MovementInputEventArgs> contextAction);
        public void UnsubscribeToMovementInputStarts(Action<MovementInputEventArgs> contextAction);
        public void UnsubscribeToMovementInputEnds(Action<MovementInputEventArgs> contextAction);
        public void Enable();
        public void Disable();
    }
}
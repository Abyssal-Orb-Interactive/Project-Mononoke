using UnityEngine;

namespace Source.Input
{
    public partial class InputManager
    {
        public class OnMovementInputChangedEventArgs
        {
            public readonly Vector2 MovementVector;

            public OnMovementInputChangedEventArgs(Vector2 movementVector)
            {
                MovementVector = movementVector;
            }
        }   
    }
}
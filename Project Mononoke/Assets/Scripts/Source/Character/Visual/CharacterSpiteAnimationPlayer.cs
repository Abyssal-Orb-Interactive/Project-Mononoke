using Base.Input;
using UnityEngine;

namespace Source.Character.Visual
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharacterSpiteAnimationPlayer : MonoBehaviour
    {
        private SpriteRenderer _sprite = null;
        private InputHandler _inputHandler = null;
        private MovementDirection _facing = MovementDirection.Stay;

        private void OnValidate()
        {
            _sprite ??= GetComponent<SpriteRenderer>();
        }

        public void Initialize(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void OnMovementInputChanged(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            
        }


        private void FlipSpritByX(MovementDirection newFacing)
        {
            if((_facing == MovementDirection.East || _facing == MovementDirection.NorthEast || _facing == MovementDirection.SouthEast) && (newFacing == MovementDirection.West || newFacing == MovementDirection.NorthWest || _facing == MovementDirection.SouthWest)){
                
            }
        }
    }
}


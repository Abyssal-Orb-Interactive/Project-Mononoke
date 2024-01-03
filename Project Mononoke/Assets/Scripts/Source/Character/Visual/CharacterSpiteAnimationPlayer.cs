using System;
using Base.Input;
using UnityEngine;

namespace Source.Character.Visual
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class CharacterSpiteAnimationPlayer : MonoBehaviour, IDisposable
    {
        private SpriteRenderer _sprite = null;
        private Animator _animator = null;
        private InputHandler _inputHandler = null;
        private MovementDirection _facing = MovementDirection.Stay;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
            _sprite ??= GetComponent<SpriteRenderer>();
        }

        public void Initialize(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            StartInputHandling();
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            if((MovementDirection)args.ActionData == MovementDirection.Stay) return;
            //FlipSpritByXIfNecessary((MovementDirection)args.ActionData);
            _facing = (MovementDirection)args.ActionData;
            Debug.Log((float)_facing);
            SetAnimatorFacingParameter();
        }

        private void StartInputHandling()
        {
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

        public void SetAnimatorFacingParameter()
        {
            _animator.SetFloat("Facing", MovementDirectionToAnimatorParameter());   
        }

        private float MovementDirectionToAnimatorParameter(){
            return _facing switch
            {
                MovementDirection.North => 0f,
                MovementDirection.NorthEast => 0.143f,
                MovementDirection.East => 0.286f,
                MovementDirection.SouthEast => 0.429f,
                MovementDirection.South => 0.572f,
                MovementDirection.SouthWest => 0.715f,
                MovementDirection.West => 0.858f,
                MovementDirection.NorthWest => 1f,
                _ => 0f
            };
        }


        private void FlipSpritByXIfNecessary(MovementDirection newFacing)
        {
            if (IsHorizontalFacingChanges(newFacing))
            {
                _sprite.flipX = !_sprite.flipX;
            }
        }

        private bool IsHorizontalFacingChanges(MovementDirection newFacing)
        {
            Debug.Log(_facing);
            Debug.Log(FacedToEast());
            Debug.Log(FacedToWestIs(newFacing));
            return ((FacedToEast() && FacedToWestIs(newFacing)) || (FacedToWest() && FacedToEastIs(newFacing)));
        }

        private bool FacedToEast()
        {
            return _facing == MovementDirection.East || _facing == MovementDirection.NorthEast || _facing == MovementDirection.SouthEast || _facing == MovementDirection.Stay;
        }

        private bool FacedToWestIs(MovementDirection newFacing)
        {
            return newFacing == MovementDirection.West || newFacing == MovementDirection.NorthWest || newFacing == MovementDirection.SouthWest;
        }

        private bool FacedToWest()
        {
            return _facing == MovementDirection.West || _facing == MovementDirection.NorthWest || _facing == MovementDirection.SouthWest;
        }

         private bool FacedToEastIs(MovementDirection newFacing)
        {
            return newFacing == MovementDirection.East || newFacing == MovementDirection.NorthEast || newFacing == MovementDirection.SouthEast ||  newFacing == MovementDirection.Stay;
        }
    }
}


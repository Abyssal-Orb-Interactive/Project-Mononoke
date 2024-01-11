using System;
using Base.Input;
using UnityEngine;
using VContainer;

namespace Source.Character.Visual
{
    [RequireComponent(typeof(Animator))]
    public class CharacterSpiteAnimationPlayer : MonoBehaviour, IDisposable
    {
        private Animator _animator = null;
        private InputHandler _inputHandler = null;
        private MovementDirection _facing = MovementDirection.Stay;

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }

        [Inject] public void Initialize(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            StartInputHandling();
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            if((MovementDirection)args.ActionData == MovementDirection.Stay) 
            {
                _animator.SetBool("RunDesired", false);
                return;
            }

            _facing = (MovementDirection)args.ActionData;
            SetAnimatorFacingParameter();
            _animator.SetBool("RunDesired", true);
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
    }
}


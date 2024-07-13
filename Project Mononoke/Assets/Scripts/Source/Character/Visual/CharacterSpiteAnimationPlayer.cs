using System;
using Base.Input;
using Source.BattleSystem.Visual;
using Source.Character.Movement;
using UnityEngine;
using VContainer;

namespace Source.Character.Visual
{
    [RequireComponent(typeof(Animator))]
    public class CharacterSpiteAnimationPlayer : MonoBehaviour, IDisposable
    {
        [SerializeField] private Animator _animator = null;
        private IsoCharacterMover _characterMover = null;
        [SerializeField] private AttackTrigger _attackTrigger = null;
        private MovementDirection _facing;
        
        private static readonly int Facing = Animator.StringToHash("Facing");
        private static readonly int RunDesired = Animator.StringToHash("RunDesired");
        private static readonly int AttackDesired = Animator.StringToHash("AttackDesired");

        private void OnValidate()
        {
            GetAnimator();
        }

        private void GetAnimator()
        {
            _animator ??= GetComponent<Animator>();
        }

        [Inject] public void Initialize(IsoCharacterMover characterMover)
        {
            GetAnimator();
            _characterMover = characterMover;
            StartMovementHandling();
            StartAttackHandling();
        }

        private void StartAttackHandling()
        {
            if(_attackTrigger == null) return;
            _attackTrigger.AttackStart += OnAttackStart;
            _attackTrigger.AttackEnds += OnAttackEnd;
        }

        private void OnAttackEnd()
        {
            _animator.SetBool(AttackDesired, false);
        }

        private void OnAttackStart()
        {
            _animator.SetBool(AttackDesired, true);
        }

        private void OnMovementChange(object sender, IsoCharacterMover.MovementActionEventArgs args)
        {
            if(_animator == null) GetAnimator();
            _facing = args.Facing;
            SetAnimatorFacingParameter();
            if(args.Status == IsoCharacterMover.MovementStatus.Ended) 
            {
                _animator.SetBool(RunDesired, false);
                return;
            }
            _animator.SetBool(RunDesired, true);
        }

        private void StartMovementHandling()
        {
            _characterMover.MovementChanged += OnMovementChange;
        }
        
        private void StopMovementHandling()
        {
            _characterMover.MovementChanged -= OnMovementChange;
        }
        
        private void OnEnable()
        {
            if(_characterMover == null) return;
            StartMovementHandling();
        }

        private void OnDisable()
        {
            if(_characterMover == null) return;
            StopMovementHandling();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            StopMovementHandling();
            _characterMover = null;
        }

        private void SetAnimatorFacingParameter()
        {
            _animator.SetFloat(Facing, MovementDirectionToAnimatorParameter());
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


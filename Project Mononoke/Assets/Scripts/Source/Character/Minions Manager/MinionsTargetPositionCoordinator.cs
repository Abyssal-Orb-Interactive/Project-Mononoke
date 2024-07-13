using System;
using Base.Input;
using Source.Character.Movement;
using UnityEngine;
using VContainer;

namespace Source.Character.Minions_Manager
{
    public class MinionsTargetPositionCoordinator : MonoBehaviour
    {
        [SerializeField] private float _targetDesignationRadius = 3f;
        [SerializeField] private Transform _targetTransform = null;

        private IsoCharacterMover _mover = null;
        private MovementDirection _facing = MovementDirection.East;

        public event Action<Vector3> TargetPositionChanged = null;

        [Inject]
        public void Initialize(IsoCharacterMover mover)
        {
            _mover = mover;
            _mover.MovementChanged += OnMovementChanged;
        }

        private void OnMovementChanged(object sender, IsoCharacterMover.MovementActionEventArgs args)
        {
            _facing = args.Facing;
        }

        public void ChangeTargetPosition()
        {
            _targetTransform.position = CalculateTargetPosition();
            TargetPositionChanged?.Invoke(_targetTransform.position);
        }

        private Vector3 CalculateTargetPosition()
        {
            
            var isoOneFacingVector = DirectionToVector3IsoConverter.ToVector(_facing);
            var coordinatorPosition = _mover.GetCurrentLogicalPosition();
            return coordinatorPosition + isoOneFacingVector * _targetDesignationRadius;
        }
    }
}
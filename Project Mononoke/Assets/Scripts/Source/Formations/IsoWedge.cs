using System.Collections.Generic;
using Base.Input;
using Base.Math;
using Source.Character.Movement;
using UnityEngine;
using VContainer;

namespace Source.Formations
{
    public class IsoWedge : Formation
    {
        [SerializeField] private int _rowsNumber = 3;
        [SerializeField] private float _evenRowsOffset = 0f;
        [SerializeField] private IsoCharacterMover _followingCharacter = null;

        private MovementDirection _facing = MovementDirection.North;
        private IEnumerable<Vector3> _positions = null;

        [Inject]
        public void Initialize(IsoCharacterMover followingCharacter)
        {
            _followingCharacter = followingCharacter;
            _followingCharacter.MovementChanged += RotateFormation;
        }

        private void RotateFormation(object sender, IsoCharacterMover.MovementActionEventArgs args)
        {
            if(_facing == args.Facing) return;
            _facing = args.Facing;
            RelocateFormationOneCellBehindCharacter();
            _positions ??= CalculateFormationPositions();
            foreach (var formationPosition in _positions)
            { 
                RotateFormationPosition(formationPosition);
            }
        }

        public override IEnumerable<Vector3> GetFormationPositions()
        {
            return _positions ??= CalculateFormationPositions();
        }

        private IEnumerable<Vector3> CalculateFormationPositions()
        {
            var topOffset = new Vector3(0.5f, _rowsNumber - 0.5f, 0);

            for (var y = _rowsNumber - 1; y >= 0; y--)
            {
                for (var x = y * -1; x <= y; x++)
                {
                    if(_isHollow && y < _rowsNumber - 1 && x > y * -1 && x < y) continue;
                    var position = new Vector3(x + (y % 2 == 0 ? 0 : _evenRowsOffset), _rowsNumber - 1 - y, 0);
                    position -= topOffset;
                    position += GetDisorderedOffsetFor(position);
                    position *= _positionsDistance;
                    position = RotateFormationPosition(position);
                    var worldPosition = transform.position;
                    var worldCartesianPosition = new Vector3Iso(worldPosition.x, worldPosition.y, worldPosition.z).ToCartesian();
                    var cartesianPosition = worldCartesianPosition + position;
                    yield return cartesianPosition;
                }
            }
        }

        private Vector3 RotateFormationPosition(Vector3 position)
        {
            position = DirectionToQuaternionConverter.GetQuaternionFor(_facing) * position;
            return position;
        }

        private void RelocateFormationOneCellBehindCharacter()
        {
            var oneIsoVector = DirectionToVector3IsoConverter.ToVector(DirectionReverser.Reverse(_facing));
            transform.localPosition = oneIsoVector;
        }
    }
}
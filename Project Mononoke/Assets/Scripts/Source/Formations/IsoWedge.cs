using System.Collections.Generic;
using Base.Input;
using Base.Input.Actions;
using Base.Math;
using Source.Character.Movement;
using UnityEngine;

namespace Source.Formations
{
    public class IsoWedge : Formation
    {
        [SerializeField] private int _rowsNumber = 3;
        [SerializeField] private float _evenRowsOffset = 0f;

        public override IEnumerable<Vector3> GetFormationPositions()
        {
            var topOffset = new Vector3(0.5f, _rowsNumber - 0.5f, 0);
            var oneIsoVector = DirectionToVector3IsoConverter.ToVector(MovementDirection.NorthEast);
            transform.localPosition = oneIsoVector;
            
            for (var y = _rowsNumber - 1; y >= 0; y--)
            {
                for (var x = y * -1; x <= y; x++)
                {
                    if(_isHollow && y < _rowsNumber - 1 && x > y * -1 && x < y) continue;
                    var position = new Vector3(x + (y % 2 == 0 ? 0 : _evenRowsOffset), _rowsNumber - 1 - y, 0);
                    position -= topOffset;
                    position += GetDisorderedOffsetFor(position);
                    position *= _positionsDistance;
                    position = DirectionToQuaternionConverter.GetQuaternionFor(MovementDirection.SouthWest) * position;
                    var worldPosition = transform.position;
                    var worldCartesianPosition = new Vector3Iso(worldPosition.x, worldPosition.y, worldPosition.z).ToCartesian();
                    var cartesianPosition = worldCartesianPosition + position;
                    yield return cartesianPosition;
                }
            }
        }
    }
}
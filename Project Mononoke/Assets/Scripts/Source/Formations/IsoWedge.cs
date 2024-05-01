using System.Collections.Generic;
using Base.Math;
using UnityEngine;

namespace Source.Formations
{
    public class IsoWedge : Formation
    {
        [SerializeField] private int _rowsNumber = 3;
        [SerializeField] private float _evenRowsOffset = 0f;

        public override IEnumerable<Vector3> GetFormationPositions()
        {
            var middleOffset = new Vector3(0, (_rowsNumber - 1) * 0.5f, 0);

            for (var y = _rowsNumber - 1; y >= 0; y--)
            {
                for (var x = y * -1; x <= y; x++)
                {
                    if(_isHollow && y < _rowsNumber - 1 && x > y * -1 && x < y) continue;
                    var position = new Vector3(x + (y % 2 == 0 ? 0 : _evenRowsOffset), _rowsNumber - 1 - y, 0);
                    position -= middleOffset;
                    position += GetDisorderedOffsetFor(position);
                    position *= _positionsDistance;
                    var isoPosition = new Vector3Iso(position);
                    yield return new Vector3(isoPosition.X, isoPosition.Y, isoPosition.Z);
                }
            }
        }
    }
}
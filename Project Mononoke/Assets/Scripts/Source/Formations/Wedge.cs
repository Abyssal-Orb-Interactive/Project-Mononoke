using System.Collections.Generic;
using UnityEngine;

namespace Source.Formations
{
    public class Wedge : Formation
    {
        private int _rowsNumber = 3;
        private float _evenRowsOffset = 0f;
        private List<Vector3> _positions = null;

        public Wedge(int rowsNumber, float evenRowsOffset = 0f)
        {
            _rowsNumber = rowsNumber;
            _evenRowsOffset = evenRowsOffset;
        }

        public override IEnumerable<Vector3> GetFormationPositions()
        {
            if(_positions == null) CalculateFormationPositions();
            return _positions;
        }

        private void CalculateFormationPositions()
        {
            var topOffset = new Vector3(0.5f, _rowsNumber - 0.5f, 0);
            _positions = new List<Vector3>();
            for (var y = _rowsNumber - 1; y >= 0; y--)
            {
                for (var x = y * -1; x <= y; x++)
                {
                    if(_isHollow && y < _rowsNumber - 1 && x > y * -1 && x < y) continue;
                    var position = new Vector3(x + (y % 2 == 0 ? 0 : _evenRowsOffset), _rowsNumber - 1 - y, 0);
                    position -= topOffset;
                    position += GetDisorderedOffsetFor(position);
                    position *= _positionsDistance;
                    _positions.Add(position);
                }
            }
        }
    }
}
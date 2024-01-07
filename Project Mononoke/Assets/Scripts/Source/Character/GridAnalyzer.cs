using Base.Grid;
using Base.Input;
using UnityEngine;

namespace Source.Character
{
    public class GridAnalyzer
    {
        private readonly GroundGrid _grid = null;

        public GridAnalyzer(GroundGrid grid)
        {
            _grid = grid;
        }

        public bool IsNextCellMovable(Vector3 currentPosition, MovementDirection facing)
        {
            var roundedCurrentPosition = new Vector3Int(Mathf.FloorToInt(currentPosition.x), Mathf.FloorToInt(currentPosition.y), Mathf.FloorToInt(currentPosition.z));
            var unitVector = Vector3Int.zero;

            unitVector = facing switch
            {
                MovementDirection.North => new(0, 1),
                MovementDirection.NorthEast => new(1, 1),
                MovementDirection.East => new(1, 0),
                MovementDirection.SouthEast => new(1, -1),
                MovementDirection.South => new(0, -1),
                MovementDirection.SouthWest => new(-1, -1),
                MovementDirection.West => new(-1, 0),
                MovementDirection.NorthWest => new(-1, 1),
                _ => Vector3Int.zero,
            };

            var targetPosition = roundedCurrentPosition + unitVector;
            return _grid.IsCellPassableAt(targetPosition);
        }
    }
}

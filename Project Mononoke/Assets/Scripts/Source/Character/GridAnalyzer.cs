using Base.Grid;
using Base.Input;
using Base.Math;
using JetBrains.Annotations;
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

        public bool IsNextCellMovable(Vector3 entityPosition, MovementDirection entityFacing)
        {
            Vector3Int NextCellCoordinate = GetNextCellCoordinate(entityPosition, entityFacing);
            return _grid.IsCellPassableAt(NextCellCoordinate);
        }

        public Vector3Int GetNextCellCoordinate(Vector3 entityCoordinate, MovementDirection entityFacing)
        {
            Vector3Int currentCellCoordinate = WorldToGrid(entityCoordinate);
            Vector3Int unitVector = GetNextCellOffsetUsing(entityFacing);

            var nextCellCoordinate = currentCellCoordinate + unitVector;
            return nextCellCoordinate;
        }

        private Vector3Int WorldToGrid(Vector3 worldCoordinate)
        {
            var isometricCoordinate = new Vector3Iso(worldCoordinate.x, worldCoordinate.y, worldCoordinate.z);
            var cartesianCoordinate = Vector3Iso.ToCartesian(isometricCoordinate);
            var roundedCoordinate = new Vector3Int(Mathf.RoundToInt(cartesianCoordinate.x), Mathf.RoundToInt(cartesianCoordinate.y), Mathf.RoundToInt(cartesianCoordinate.z));
            return roundedCoordinate;
        }

        private Vector3Int GetNextCellOffsetUsing(MovementDirection entityFacing)
        {
            return entityFacing switch
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
        }

        public Vector3 GetNextCellSizes(Vector3 entityPosition, MovementDirection entityFacing)
        {
            Vector3Int nextCellCoordinate = GetNextCellCoordinate(entityPosition, entityFacing);
            return _grid.GetSizesOfCellAt(nextCellCoordinate);
        }

        public bool IsWithinCellBounds(Vector3 entityPosition)
        {
            var currentCellCoordinate = WorldToGrid(entityPosition);
            var currentCellSizes = _grid.GetSizesOfCellAt(currentCellCoordinate);
            return entityPosition.x >= currentCellCoordinate.x && entityPosition.x < currentCellCoordinate.x + currentCellSizes.x
                && entityPosition.y >= currentCellCoordinate.y && entityPosition.y < currentCellCoordinate.y + currentCellSizes.y;
        }
    }
}

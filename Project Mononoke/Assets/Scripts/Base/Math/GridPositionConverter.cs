using UnityEngine;

namespace Base.Math
{
    public static class GridPositionConverter
    {
        public static Vector3 GetWorldPosition(InPlaneCoordinateInt coordinate, Grid.Grid grid)
        {
            return CoordinateToVectorConverter.ConvertInPlaneCoordinateIntToVector3(coordinate) * grid.CellSize + CoordinateToVectorConverter.ConvertInSpaceCoordinateToVector3(grid.OriginPosition);
        }
        
        public static Vector3 GetCellCenterWorldPosition(InPlaneCoordinateInt coordinate, Grid.Grid grid)
        {
            const float CENTER_OFFSET = 0.5f;

            return GetWorldPosition(coordinate, grid) + GetCellCenterWorldOffset(CENTER_OFFSET, grid);
        }

        private static Vector3 GetCellCenterWorldOffset(float centerOffset, Grid.Grid grid)
        {
            return GetCellSizes(grid) * centerOffset;
        }

        private static Vector3 GetCellSizes(Grid.Grid grid)
        {
            return new Vector3(grid.CellSize, grid.CellSize);
        }

        public static InPlaneCoordinateInt GetCoordinateInGrid(Vector3 worldPosition, Grid.Grid grid)
        {
            var originPos = CoordinateToVectorConverter.ConvertInSpaceCoordinateToVector3(grid.OriginPosition);
            var x = Mathf.FloorToInt((worldPosition.x - originPos.x) / grid.CellSize);
            var y = Mathf.FloorToInt((worldPosition.y - originPos.y)/ grid.CellSize);

            return new InPlaneCoordinateInt(x, y);
        }
    }
}
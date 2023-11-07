using UnityEngine;

namespace Base.Math
{
    public static class GridPositionConverter
    {
        public static Vector3 GetWorldPosition(InPlaneCoordinateInt coordinate, float cellSize, InSpaceCoordinate gridOriginPosition)
        {
            return CoordinateToVectorConverter.ConvertInPlaneCoordinateIntToVector3(coordinate) * cellSize + CoordinateToVectorConverter.ConvertInSpaceCoordinateToVector3(gridOriginPosition);
        }
        
        public static Vector3 GetCellCenterWorldPosition(InPlaneCoordinateInt coordinate, float cellSize, InSpaceCoordinate gridOriginPosition)
        {
            const float CENTER_OFFSET = 0.5f;

            return GetWorldPosition(coordinate, cellSize, gridOriginPosition) + GetCellCenterWorldOffset(CENTER_OFFSET, cellSize);
        }

        private static Vector3 GetCellCenterWorldOffset(float centerOffset, float cellSize)
        {
            return GetCellSizes(cellSize) * centerOffset;
        }

        private static Vector3 GetCellSizes(float cellSize)
        {
            return new Vector3(cellSize, cellSize);
        }

        public static InPlaneCoordinateInt GetCoordinateInGrid(Vector3 worldPosition, float cellSize, InSpaceCoordinate gridOriginPosition)
        {
            var originPosition = CoordinateToVectorConverter.ConvertInSpaceCoordinateToVector3(gridOriginPosition);
            var x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
            var y = Mathf.FloorToInt((worldPosition.y - originPosition.y)/ cellSize);

            return new InPlaneCoordinateInt(x, y);
        }
    }
}
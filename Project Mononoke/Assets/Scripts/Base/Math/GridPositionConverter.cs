using UnityEngine;

namespace Base.Math
{
    public class GridPositionConverter
    {
        private readonly float _cellSize;
        private readonly Vector3 _gridOriginPosition;

        private GridPositionConverter(){}

        public GridPositionConverter(float cellSize, Vector3 gridOriginPosition)
        {
            _cellSize = cellSize;
            _gridOriginPosition = gridOriginPosition;
        }
        
        public Vector3 GetWorldPosition(InPlaneCoordinateInt coordinate)
        {
            return new Vector3(coordinate.X, coordinate.Y) * _cellSize + _gridOriginPosition;
        }
        
        public Vector3 GetCellCenterWorldPosition(InPlaneCoordinateInt coordinate)
        {
            const float CENTER_OFFSET = 0.5f;

            return GetWorldPosition(coordinate) + GetCellCenterWorldOffset(CENTER_OFFSET);
        }

        private Vector3 GetCellCenterWorldOffset(float centerOffset)
        {
            return GetCellSizes() * centerOffset;
        }

        private Vector3 GetCellSizes()
        {
            return new Vector3(_cellSize, _cellSize);
        }

        public InPlaneCoordinateInt GetCoordinateInGrid(Vector3 worldPosition)
        {
            var x = Mathf.FloorToInt((worldPosition.x - _gridOriginPosition.x) / _cellSize);
            var y = Mathf.FloorToInt((worldPosition.y - _gridOriginPosition.y)/ _cellSize);

            return new InPlaneCoordinateInt(x, y);
        }
    }
}
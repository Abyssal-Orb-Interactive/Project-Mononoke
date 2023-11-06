using System;
using System.Collections.Generic;
using Base.Math;
using Base.Utils;
using TMPro;
using UnityEngine;

namespace Base.Grid
{
   
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform _cellTextContainer;

        private const float FONT_SIZE = 20f;
        private const TextAlignmentOptions TEXT_AGLIMENT = TextAlignmentOptions.Center;
        private readonly TextStyle TEXT_STYLE = new (FONT_SIZE, TextAlignment: TEXT_AGLIMENT);

        private Grid _grid;
        private GridPositionConverter _positionConverter;
        private Dictionary<InPlaneCoordinateInt, TextMeshPro> _debugTexts;

        public void Visualize(Grid grid)
        {
            _grid = grid ?? throw new GridVisualizationException("You can't visualize null grid.");
            _debugTexts?.Clear();
            _debugTexts = new Dictionary<InPlaneCoordinateInt, TextMeshPro>();
            _positionConverter = new GridPositionConverter(_grid.CellSize, _grid.OriginPosition);
            
            foreach (var coordinate in grid.GetCellsCoordinates())
            {
                InstantiateTextOnCoordinate(coordinate);
                DrawCornerCellWithGizmosWhiteLinesOn100Seconds(coordinate);
            }
            DrawGridUpperRightCornerWithGizmosWhiteLinesOn100Seconds();
        }

        private void InstantiateTextOnCoordinate(InPlaneCoordinateInt coordinate)
        {
            _debugTexts.Add(coordinate,
                TextMeshProFabric.CreateTextInWorld(_grid.GetCellValue(coordinate)
                        .ToString(),
                    new TextProperties(TEXT_STYLE,
                        _cellTextContainer,
                        _positionConverter.GetCellCenterWorldPosition(coordinate))));
        }

        private void DrawCornerCellWithGizmosWhiteLinesOn100Seconds(InPlaneCoordinateInt coordinate)
        {
            const float GIZMOS_DURATION_TIME = 100f;
            var GIZMOS_COLOR = Color.white;
            
            DrawVerticalLine(coordinate, GIZMOS_COLOR, GIZMOS_DURATION_TIME);
            DrawHorizontalLine(coordinate, GIZMOS_COLOR, GIZMOS_DURATION_TIME);
        }

        private void DrawHorizontalLine(InPlaneCoordinateInt coordinate, Color gizmosColor, float gizmosDurationTime)
        {
            Debug.DrawLine(_positionConverter.GetWorldPosition(coordinate),
                _positionConverter.GetWorldPosition(GetNextHorizontalCoordinate(coordinate)),
                gizmosColor,
                gizmosDurationTime);
        }

        private void DrawVerticalLine(InPlaneCoordinateInt coordinate, Color gizmosColor, float gizmosDurationTime)
        {
            Debug.DrawLine(_positionConverter.GetWorldPosition(coordinate),
                _positionConverter.GetWorldPosition(GetNextVerticalCoordinate(coordinate)),
                gizmosColor,
                gizmosDurationTime);
        }

        private static InPlaneCoordinateInt GetNextHorizontalCoordinate(InPlaneCoordinateInt coordinate)
        {
            return new InPlaneCoordinateInt(coordinate.X + 1, coordinate.Y);
        }

        private InPlaneCoordinateInt GetNextVerticalCoordinate(InPlaneCoordinateInt coordinate)
        {
            return new InPlaneCoordinateInt(coordinate.X, coordinate.Y + 1);
        }
        
        private void DrawGridUpperRightCornerWithGizmosWhiteLinesOn100Seconds()
        {
            const float GIZMOS_DURATION_TIME = 100f;
            var GIZMOS_COLOR = Color.white;
            
            DrawVerticalLine(GIZMOS_COLOR, GIZMOS_DURATION_TIME);
            DrawHorizontalLine(GIZMOS_COLOR, GIZMOS_DURATION_TIME);
        }

        private void DrawHorizontalLine(Color gizmosColor, float gizmosDurationTime)
        {
            var startPosition = new InPlaneCoordinateInt(_grid.Sizes.X, 0);
            Debug.DrawLine(_positionConverter.GetWorldPosition(startPosition),
                _positionConverter.GetWorldPosition(_grid.Sizes), gizmosColor, gizmosDurationTime);
        }

        private void DrawVerticalLine(Color gizmosColor, float gizmosDurationTime)
        {
            var startPosition = new InPlaneCoordinateInt(0, _grid.Sizes.Y);
            Debug.DrawLine(_positionConverter.GetWorldPosition(startPosition),
                _positionConverter.GetWorldPosition(_grid.Sizes), gizmosColor, gizmosDurationTime);
        }

        private void SetCellValue(InPlaneCoordinateInt coordinate,int value)
        {
            if (_grid.TrySetValue(coordinate, value))
                _debugTexts[coordinate].GetComponent<TMP_Text>().text = value.ToString();
        }

        public void SetCellValue(Vector3 worldPosition, int value)
        {
            var coordinate = _positionConverter.GetCoordinateInGrid(worldPosition);
            SetCellValue(coordinate, value);
        }

        public int GetCellValue(Vector3 worldPosition)
        {
            var coordinate = _positionConverter.GetCoordinateInGrid(worldPosition);
            return GetCellValue(coordinate);
        }
        
        private int GetCellValue(InPlaneCoordinateInt coordinate)
        {
            return _grid.GetCellValue(coordinate);
        }
    }
    
    public class GridVisualizationException : Exception
    {
        public GridVisualizationException() { }

        public GridVisualizationException(string message)
            : base(message) { }

        public GridVisualizationException(string message, Exception inner)
            : base(message, inner) { }
    }
}

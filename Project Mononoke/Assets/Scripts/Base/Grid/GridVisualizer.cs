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
        [SerializeField] private GameObject _cellTextContainer;
        [SerializeField] private bool _debugMode = false;

        private const float FONT_SIZE = 20f;
        private const TextAlignmentOptions TEXT_AGLIMENT = TextAlignmentOptions.Center;
        private readonly TextStyle TEXT_STYLE = new (FONT_SIZE, TextAlignment: TEXT_AGLIMENT);
        
        private Dictionary<InPlaneCoordinateInt, TextMeshPro> _debugTexts;
        private InPlaneCoordinateInt _gridSizes;
        private InSpaceCoordinate _gridOrigin;
        private float _cellSize;
        
        public void Visualize<TCellValue>(IGrid<TCellValue> grid)
        {
             if(grid == null) throw new GridVisualizationException("You can't visualize null grid.");
             
             _gridSizes = grid.Sizes;
             _gridOrigin = grid.OriginPosition;
             _cellSize = grid.CellArea;
             
            _debugTexts?.Clear();
            
            if(!_debugMode) return;
            
            _debugTexts = new Dictionary<InPlaneCoordinateInt, TextMeshPro>();
            
            foreach (var coordinate in grid.GetCellsCoordinates())
            {
                InstantiateTextOnCoordinate(coordinate, grid.GetCellValue(coordinate));
                DrawCornerCellWithGizmosWhiteLinesOn100Seconds(coordinate);
            }
            DrawGridUpperRightCornerWithGizmosWhiteLinesOn100Seconds();
            
            grid.SubscribeOnCellValueChanged(OnValueChanged);
        }

        public void OnValueChanged<TCellValue>(object source, Grid<TCellValue>.OnGridValueChangedEventArgs args)
        {
            _debugTexts[args.Coordinate].text = args.Value.ToString();
        }

        private void InstantiateTextOnCoordinate<TCellValue>(InPlaneCoordinateInt coordinate, TCellValue value)
        {
            _debugTexts.Add(coordinate,
                TextMeshProFabric.CreateTextInWorld(value.ToString(),
                    new TextProperties(TEXT_STYLE,
                        _cellTextContainer.transform,
                        GridPositionConverter.GetCellCenterWorldPosition(coordinate, _cellSize, _gridOrigin))));
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
            Debug.DrawLine(GridPositionConverter.GetWorldPosition(coordinate, _cellSize, _gridOrigin),
                GridPositionConverter.GetWorldPosition(GetNextHorizontalCoordinate(coordinate), _cellSize, _gridOrigin),
                gizmosColor,
                gizmosDurationTime);
        }

        private void DrawVerticalLine(InPlaneCoordinateInt coordinate, Color gizmosColor, float gizmosDurationTime)
        {
            Debug.DrawLine(GridPositionConverter.GetWorldPosition(coordinate,  _cellSize, _gridOrigin),
                GridPositionConverter.GetWorldPosition(GetNextVerticalCoordinate(coordinate), _cellSize, _gridOrigin),
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
            var startPosition = new InPlaneCoordinateInt(_gridSizes.X, 0);
            Debug.DrawLine(GridPositionConverter.GetWorldPosition(startPosition, _cellSize, _gridOrigin),
                GridPositionConverter.GetWorldPosition(_gridSizes, _cellSize, _gridOrigin), gizmosColor, gizmosDurationTime);
        }

        private void DrawVerticalLine(Color gizmosColor, float gizmosDurationTime)
        {
            var startPosition = new InPlaneCoordinateInt(0, _gridSizes.Y);
            Debug.DrawLine(GridPositionConverter.GetWorldPosition(startPosition, _cellSize, _gridOrigin),
                GridPositionConverter.GetWorldPosition(_gridSizes, _cellSize, _gridOrigin), gizmosColor, gizmosDurationTime);
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

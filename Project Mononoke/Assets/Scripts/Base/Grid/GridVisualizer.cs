using System;
using Base.Math;
using Base.Utils;
using TMPro;
using UnityEditor.Graphs;
using UnityEngine;

namespace Base.Grid
{
   
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 10f;
        [SerializeField] private Transform _cellTextContainer;

        private const float FONT_SIZE = 20f;
        private const TextAlignmentOptions TEXT_AGLIMENT = TextAlignmentOptions.Center;
        private readonly TextStyle TEXT_STYLE = new (FONT_SIZE, TextAlignment: TEXT_AGLIMENT);
        
        public void Visualize(Grid grid)
        {
            if (grid == null) throw new GridVisualizationException("You can't visualize null grid.");
            
            foreach (var coordinate in grid.GetCellsCoordinates())
            {
                InstantiateTextOnCoordinate(coordinate);
                DrawCornerCellWithGizmosWhiteLinesOn100Seconds(coordinate);
            }
            
            DrawGridUpperRightCornerWithGizmosWhiteLinesOn100Seconds(grid);
        }

        private void InstantiateTextOnCoordinate(InPlaneCoordinateInt coordinate)
        {
            TextMeshProFabric.CreateTextInWorld(GetDisplayableStringRepresentationOfCoordinate(coordinate),
                new TextProperties(TEXT_STYLE,
                    _cellTextContainer,
                    GetCellCenterWorldPosition(coordinate)));
        }

        private static string GetDisplayableStringRepresentationOfCoordinate(InPlaneCoordinateInt coordinate)
        {
            return $"{coordinate.X} , {coordinate.Y}";
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
            Debug.DrawLine(GetWorldPosition(coordinate),
                GetWorldPosition(GetNextHorizontalCoordinate(coordinate)),
                gizmosColor,
                gizmosDurationTime);
        }

        private void DrawVerticalLine(InPlaneCoordinateInt coordinate, Color gizmosColor, float gizmosDurationTime)
        {
            Debug.DrawLine(GetWorldPosition(coordinate),
                GetWorldPosition(GetNextVerticalCoordinate(coordinate)),
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
        
        private void DrawGridUpperRightCornerWithGizmosWhiteLinesOn100Seconds(Grid grid)
        {
            const float GIZMOS_DURATION_TIME = 100f;
            var GIZMOS_COLOR = Color.white;
            
            DrawVerticalLine(grid, GIZMOS_COLOR, GIZMOS_DURATION_TIME);
            DrawHorizontalLine(grid, GIZMOS_COLOR, GIZMOS_DURATION_TIME);
        }

        private void DrawHorizontalLine(Grid grid, Color gizmosColor, float gizmosDurationTime)
        {
            Debug.DrawLine(GetWorldPosition(new InPlaneCoordinateInt(grid.Sizes.X, 0)), GetWorldPosition(grid.Sizes), gizmosColor, gizmosDurationTime);
        }

        private void DrawVerticalLine(Grid grid, Color gizmosColor, float gizmosDurationTime)
        {
            Debug.DrawLine(GetWorldPosition(new InPlaneCoordinateInt(0, grid.Sizes.Y)), GetWorldPosition(grid.Sizes), gizmosColor, gizmosDurationTime);
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

        public Vector3 GetWorldPosition(InPlaneCoordinateInt coordinate)
        {
            return new Vector3(coordinate.X, coordinate.Y) * _cellSize;
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

using System;
using Base.Math;
using Base.Utils;
using TMPro;
using UnityEngine;

namespace Base.Grid
{
   
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private float _cellSize = 10f;
        
        public void VisualizeTextOn(Grid grid)
        {
            if (grid == null) throw new GridVisualizationException("You can't visualize null grid.");
            
            foreach (var coordinate in grid.GetCellsCoordinates())
            {
                TextMeshProFabric.CreateTextInWorld(new string($"{coordinate.X} , {coordinate.Y}"),
                    new TextProperties(new TextStyle(20,
                            TextAlignment: TextAlignmentOptions.Center),
                        null,
                        GetWorldPosition(coordinate)));
            }
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

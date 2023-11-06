using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Math;
using JetBrains.Annotations;
using UnityEngine;

namespace Base.Grid
{
    /// <summary>
    /// Represents a two-dimensional grid with a specific size.
    /// </summary>
    public class Grid
    {
        public const int GRID_MAX_VAlUE = 100;
        public const int GRID_MIN_VAlUE = 0;

        private event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public InPlaneCoordinateInt Coordinate;
        }
        
        /// <summary>
        /// Gets the size of the grid.
        /// </summary>
        public InPlaneCoordinateInt Sizes { get; }
        
        public InSpaceCoordinate OriginPosition { get; }
        
        public float CellSize { get; }

        private readonly Dictionary<InPlaneCoordinateInt, int> _gridDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="Grid"/> class with the specified size.
        /// </summary>
        /// <param name="sizes">The size of the grid.</param>
        /// <param name="originPosition">The origin position of the grid in world</param>
        /// <param name="cellSize">The size of grid cell</param>
        /// <exception cref="GridConstructionException">
        /// Thrown if the size in either dimension is negative or both dimensions are zero.
        /// </exception>
        public Grid(InPlaneCoordinateInt sizes, [CanBeNull] InSpaceCoordinate originPosition = null, float cellSize = 10f)
        {
            ValidateSizes(sizes);
            ValidateCellSize(cellSize);

            Sizes = sizes;
            OriginPosition = originPosition?? new InSpaceCoordinate();
            CellSize = cellSize;
            _gridDictionary = new Dictionary<InPlaneCoordinateInt, int>();

            FillGridWithCoordinates();
        }

        private void ValidateSizes(InPlaneCoordinateInt sizes)
        {
            if (sizes.X < 0)
            {
                throw new GridConstructionException("Size X of the grid can't be negative.");
            }
            if (sizes.Y < 0)
            {
                throw new GridConstructionException("Size Y of the grid can't be negative.");
            }
            if (sizes.X == 0 || sizes.Y == 0)
            {
                throw new GridConstructionException("Sizes of the grid must be greater than 0 in both dimensions.");
            }
        }

        public void ValidateCellSize(float cellSize)
        { 
            if (cellSize <= 0f) throw new GridConstructionException("Size of cell must be bigger then 0");
        }

        private void FillGridWithCoordinates()
        {
            for (var x = 0; x < Sizes.X; x++)
            {
                for (var y = 0; y < Sizes.Y; y++)
                {
                    _gridDictionary.Add(new InPlaneCoordinateInt(x, y), 0);
                }
            }
        }

        /// <summary>
        /// Gets the number of cells in the grid.
        /// </summary>
        public int CellCount => _gridDictionary.Count;

        /// <summary>
        /// Retrieves the coordinates of all cells in the grid.
        /// </summary>
        /// <returns>An enumerable collection of cell coordinates.</returns>
        public IEnumerable<InPlaneCoordinateInt> GetCellsCoordinates()
        {
            return _gridDictionary.Keys;
        }
        
        public int GetCellValue(InPlaneCoordinateInt coordinate)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return -1;

            return _gridDictionary[coordinate];
        }

        public bool TrySetValue(InPlaneCoordinateInt coordinate, int value)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return false;
            if (value is < GRID_MIN_VAlUE or > GRID_MAX_VAlUE) return false;
            
            SetCellValue(coordinate, value);

            return true;
        }
        
        private void SetCellValue(InPlaneCoordinateInt coordinate, int value)
        {
            _gridDictionary[coordinate] = value;
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{Coordinate = coordinate});
        }

        private void AddValueToCell(InPlaneCoordinateInt coordinate, int value)
        {
            SetCellValue(coordinate, GetCellValue(coordinate) + value);
        }
        
        private void AddValueToCellSilently(InPlaneCoordinateInt coordinate, int value)
        {
            SetCellValueSilently(coordinate, GetCellValue(coordinate) + value);
        }
        
        private void SetCellValueSilently(InPlaneCoordinateInt coordinate, int value)
        {
            _gridDictionary[coordinate] = value;
        }

        public void AddValueToCells(InPlaneCoordinateInt coordinate, int value, int fullValueRange, int totalRange)
        {
            if(totalRange < fullValueRange) return;
            
            var lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));
            
            for (var x = 0; x < totalRange; x++)
            {
                for (var y = 0; y < totalRange - x; y++)
                {
                    var radius = x + y;
                    var addValueAmount = value;

                    if (radius > fullValueRange) addValueAmount -= lowerValueAmount * (radius - fullValueRange);
                    
                    AddValueToCellSilently(new InPlaneCoordinateInt(coordinate.X + x, coordinate.Y + y), addValueAmount);
                    if(x != 0) AddValueToCellSilently(new InPlaneCoordinateInt(coordinate.X - x, coordinate.Y + y), addValueAmount);
                    if(y != 0) AddValueToCellSilently(new InPlaneCoordinateInt(coordinate.X + x, coordinate.Y - y), addValueAmount);
                    if(x != 0 && y != 0) AddValueToCellSilently(new InPlaneCoordinateInt(coordinate.X - x, coordinate.Y - y), addValueAmount);
                }
            }
            
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{Coordinate = coordinate});
        }
        
        /// <summary>
        /// Returns a string representation of the grid, including its sizes, the number of cells, and the coordinates of all cells.
        /// </summary>
        /// <returns>A string representation of the grid.</returns>
        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            var start = $"Grid has sizes - {Sizes} and has {CellCount} of cells.\nCells:\n";
            strBuilder.Append(start);
            foreach (var coordinate in _gridDictionary)
            {
                strBuilder.Append($"\n{coordinate}");
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Determines whether this grid is equal to another object.
        /// </summary>
        /// <param name="obj">The object to compare with this grid.</param>
        /// <returns><see langword="true"/> if the objects are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is not Grid other) return false;
            if (Sizes != other.Sizes) return false;
            
            return CellCount == other.CellCount && _gridDictionary.Equals(other._gridDictionary);
        }
        
        /// <summary>
        /// Determines whether this grid is equal to another grid.
        /// </summary>
        /// <param name="other">The grid to compare with this grid.</param>
        /// <returns><see langword="true"/> if the grids are equal; otherwise, <see langword="false"/>.</returns>
        protected bool Equals(Grid other)
        {
            return Equals(_gridDictionary, other._gridDictionary) && Equals(Sizes, other.Sizes);
        }

        /// <summary>
        /// Gets a hash code for this grid.
        /// </summary>
        /// <returns>A hash code for the grid.</returns>Lj
        public override int GetHashCode()
        {
            return HashCode.Combine(_gridDictionary, Sizes);
        }
        
        public void SubscribeOnCellValueChanged(EventHandler<OnGridValueChangedEventArgs> handler)
        {
            OnGridValueChanged += handler;
        }

        public void UnsubscribeOnCellValueChanged(EventHandler<OnGridValueChangedEventArgs> handler)
        {
            OnGridValueChanged -= handler;
        }
    }
    
    /// <summary>
    /// Represents an exception that is thrown when grid construction fails.
    /// </summary>
    public class GridConstructionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GridConstructionException"/> class.
        /// </summary>
        public GridConstructionException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridConstructionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        public GridConstructionException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridConstructionException"/> class with a specified error message and inner exception.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="inner">The inner exception.</param>
        public GridConstructionException(string message, Exception inner)
            : base(message, inner) { }
    }
}

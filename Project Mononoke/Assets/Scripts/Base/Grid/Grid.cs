using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Math;
using JetBrains.Annotations;

namespace Base.Grid
{
    public class Grid<TCellValue>
    {
        public const int GRID_MAX_VAlUE = 100;
        public const int GRID_MIN_VAlUE = 0;

        private event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public class OnGridValueChangedEventArgs : EventArgs
        {
            public InPlaneCoordinateInt Coordinate;
            public TCellValue Value;
        }
        
        public InPlaneCoordinateInt Sizes { get; }
        
        public InSpaceCoordinate OriginPosition { get; }
        
        public float CellSize { get; }

        private readonly Dictionary<InPlaneCoordinateInt, TCellValue> _gridDictionary;

      
        public Grid(InPlaneCoordinateInt sizes, [CanBeNull] InSpaceCoordinate originPosition = null, float cellSize = 10f)
        {
            ValidateSizes(sizes);
            ValidateCellSize(cellSize);

            Sizes = sizes;
            OriginPosition = originPosition?? new InSpaceCoordinate();
            CellSize = cellSize;
            _gridDictionary = new Dictionary<InPlaneCoordinateInt, TCellValue>();

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
                    _gridDictionary.Add(new InPlaneCoordinateInt(x, y), default);
                }
            }
        }
        
        public int CellCount => _gridDictionary.Count;
        
        public IEnumerable<InPlaneCoordinateInt> GetCellsCoordinates()
        {
            return _gridDictionary.Keys;
        }
        
        public TCellValue GetCellValue(InPlaneCoordinateInt coordinate)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return default;

            return _gridDictionary[coordinate];
        }

        public bool TrySetValue(InPlaneCoordinateInt coordinate, TCellValue value)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return false;
            if (value is < GRID_MIN_VAlUE or > GRID_MAX_VAlUE) return false;
            
            SetCellValue(coordinate, value);

            return true;
        }
        
        private void SetCellValue(InPlaneCoordinateInt coordinate, TCellValue value)
        {
            _gridDictionary[coordinate] = value;
            OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{Coordinate = coordinate, Value = value});
        }
        
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
        
        public override bool Equals(object obj)
        {
            if (obj is not Grid<TCellValue> other) return false;
            if (Sizes != other.Sizes) return false;
            
            return CellCount == other.CellCount && _gridDictionary.Equals(other._gridDictionary);
        }
        
        protected bool Equals(Grid<TCellValue> other)
        {
            return Equals(_gridDictionary, other._gridDictionary) && Equals(Sizes, other.Sizes);
        }
        
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
    
    public class GridConstructionException : Exception
    {
        public GridConstructionException() { }
        
        public GridConstructionException(string message)
            : base(message) { }
        
        public GridConstructionException(string message, Exception inner)
            : base(message, inner) { }
    }
}

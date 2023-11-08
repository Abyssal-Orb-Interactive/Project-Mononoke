using System;
using System.Collections.Generic;
using System.Linq;
using Base.Math;
using JetBrains.Annotations;

namespace Base.Grid
{
    public partial class Grid<TCellValue> : IGrid<TCellValue>
    {
        private event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;

        public InPlaneCoordinateInt Sizes { get; }
        
        public InSpaceCoordinate OriginPosition { get; }
        
        public float CellArea { get; }

        private readonly Dictionary<InPlaneCoordinateInt, ICell<TCellValue>> _gridDictionary;

        private Grid(){}
        
        public Grid(InPlaneCoordinateInt sizes,  Func<TCellValue> valueCreationStrategy, [CanBeNull] InSpaceCoordinate originPosition = null, float cellArea = 10f)
        {
            ValidateSizes(sizes);
            ValidateCellSize(cellArea);

            Sizes = sizes;
            OriginPosition = originPosition?? new InSpaceCoordinate();
            CellArea = cellArea;
            _gridDictionary = new Dictionary<InPlaneCoordinateInt, ICell<TCellValue>>();

            FillGridWithCoordinates(valueCreationStrategy);
        }

        public InPlaneCoordinateInt GetCoordinateOfCell(TCellValue cellValue)
        {
            return _gridDictionary.Where(pair => pair.Value.GetValue().Equals(cellValue)).Select(pair => pair.Key).FirstOrDefault();
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

        private void ValidateCellSize(float cellSize)
        { 
            if (cellSize <= 0f) throw new GridConstructionException("Size of cell must be bigger then 0");
        }

        private void FillGridWithCoordinates(Func<TCellValue> valueCreationStrategy)
        {
            for (var x = 0; x < Sizes.X; x++)
            {
                for (var y = 0; y < Sizes.Y; y++)
                {
                    _gridDictionary.Add(new InPlaneCoordinateInt(x, y), new Cell<TCellValue>(valueCreationStrategy()));
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
            return !_gridDictionary.Keys.Contains(coordinate) ? default : _gridDictionary[coordinate].GetValue();
        }

        public bool TrySetValue(InPlaneCoordinateInt coordinate, TCellValue value)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return false;
            if (_gridDictionary[coordinate].TrySetValue(value)) OnGridValueChanged?.Invoke(this, new OnGridValueChangedEventArgs{Coordinate = coordinate, Value = value});
            return true;
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

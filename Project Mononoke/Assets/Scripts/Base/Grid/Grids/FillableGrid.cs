using System;
using System.Collections.Generic;
using System.Linq;
using Base.Math;
using JetBrains.Annotations;

namespace Base.Grid
{
    public class FillableGrid<TCellValue> : IGrid<TCellValue> where TCellValue : IFillableCellContent
    {
        public InPlaneCoordinateInt Sizes { get; }
        public InSpaceCoordinate OriginPosition { get; }
        public float CellArea { get; }
        public int CellCount => _gridDictionary.Count;
        
        private readonly Dictionary<InPlaneCoordinateInt, FillableCell<TCellValue>> _gridDictionary;
        private event EventHandler<Grid<TCellValue>.OnGridValueChangedEventArgs> OnGridValueChanged;
        
        private FillableGrid(){}
        
        public FillableGrid(InPlaneCoordinateInt sizes, Func<TCellValue> contentCreationStrategy, [CanBeNull] InSpaceCoordinate originPosition = null, float cellArea = 10f)
        {
            ValidateSizes(sizes);
            ValidateCellSize(cellArea);

            Sizes = sizes;
            OriginPosition = originPosition?? new InSpaceCoordinate();
            CellArea = cellArea;
            _gridDictionary = new Dictionary<InPlaneCoordinateInt, FillableCell<TCellValue>>();

            FillGridWithContent(contentCreationStrategy);
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

        private void FillGridWithContent(Func<TCellValue> contentCreationStrategy)
        {
            for (var x = 0; x < Sizes.X; x++)
            {
                for (var y = 0; y < Sizes.Y; y++)
                {
                    _gridDictionary.Add(new InPlaneCoordinateInt(x, y), new FillableCell<TCellValue>(contentCreationStrategy()));
                }
            }
        }

        
        public IEnumerable<InPlaneCoordinateInt> GetCellsCoordinates()
        {
            return _gridDictionary.Keys;
        }

        public TCellValue GetCellValue(InPlaneCoordinateInt coordinate)
        {
            return !_gridDictionary.Keys.Contains(coordinate) ? default : _gridDictionary[coordinate].GetValue();
        }
        
        public float GetCellValueInPercents(InPlaneCoordinateInt coordinate)
        {
            return !_gridDictionary.Keys.Contains(coordinate) ? 0f : _gridDictionary[coordinate].GetValueInPercents();
        }

        public bool TrySetValue(InPlaneCoordinateInt coordinate, TCellValue value)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return false;
            if (_gridDictionary[coordinate].TrySetValue(value)) OnGridValueChanged?.Invoke(this, new Grid<TCellValue>.OnGridValueChangedEventArgs{Coordinate = coordinate, Value = value});
            return true;
        }

        public bool TryAddValue(InPlaneCoordinateInt coordinate, TCellValue value)
        {
            if (!_gridDictionary.Keys.Contains(coordinate)) return false;
            if (_gridDictionary[coordinate].TryAddValue(value)) OnGridValueChanged?.Invoke(this, new Grid<TCellValue>.OnGridValueChangedEventArgs{Coordinate = coordinate, Value = _gridDictionary[coordinate].GetValue()});
            return true;
        }

        public void SubscribeOnCellValueChanged(EventHandler<Grid<TCellValue>.OnGridValueChangedEventArgs> handler)
        {
            OnGridValueChanged += handler;
        }

        public void UnsubscribeOnCellValueChanged(EventHandler<Grid<TCellValue>.OnGridValueChangedEventArgs> handler)
        {
            OnGridValueChanged -= handler;
        }
    }
}
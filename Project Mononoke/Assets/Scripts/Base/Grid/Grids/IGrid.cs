using System;
using System.Collections.Generic;
using Base.Math;

namespace Base.Grid
{
    public interface IGrid<TCellValue>
    {
        public InPlaneCoordinateInt Sizes { get; }
        public InSpaceCoordinate OriginPosition { get; }
        public float CellArea { get; }
        public int CellCount { get; }

        public IEnumerable<InPlaneCoordinateInt> GetCellsCoordinates();
        public TCellValue GetCellValue(InPlaneCoordinateInt coordinate);
        public bool TrySetValue(InPlaneCoordinateInt coordinate, TCellValue value);
        public void SubscribeOnCellValueChanged(EventHandler<Grid<TCellValue>.OnGridValueChangedEventArgs> handler);
        public void UnsubscribeOnCellValueChanged(EventHandler<Grid<TCellValue>.OnGridValueChangedEventArgs> handler);
    }
}
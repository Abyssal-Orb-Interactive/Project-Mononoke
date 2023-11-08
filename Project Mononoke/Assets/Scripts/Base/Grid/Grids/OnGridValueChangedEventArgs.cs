using System;
using Base.Math;

namespace Base.Grid
{
    public partial class Grid<TCellValue>
    {
        public class OnGridValueChangedEventArgs : EventArgs
        {
            public InPlaneCoordinateInt Coordinate;
            public TCellValue Value;
        }
    }
}
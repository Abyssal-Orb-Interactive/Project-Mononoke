using System;
using System.Collections.Generic;
using Base.Math;

namespace Base.Grid
{
    public class Grid
    {
        public InPlaneCoordinateInt Sizes { get; }

        private readonly List<InPlaneCoordinateInt> _coordinates;
        
        public Grid(InPlaneCoordinateInt sizes)
        {
            if (sizes.X < 0 || sizes.Y < 0) throw new GridConstructionException("Sizes of grid can't be negative");
            if (sizes is { X: 0, Y: 0 }) throw new GridConstructionException("Sizes of grid must be bigger then 0,0");
            
            Sizes = sizes;
            _coordinates = new List<InPlaneCoordinateInt>();

            for (var x = 0; x < Sizes.X; x++)
            {
                for (var y = 0; y < Sizes.Y; y++)
                {
                    _coordinates.Add(new InPlaneCoordinateInt(x, y));
                }
            }
        }

        public int CellCount()
        {
           return _coordinates.Count;
        }

        public IEnumerable<InPlaneCoordinateInt> GetCellsCoordinates()
        {
            return _coordinates;
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

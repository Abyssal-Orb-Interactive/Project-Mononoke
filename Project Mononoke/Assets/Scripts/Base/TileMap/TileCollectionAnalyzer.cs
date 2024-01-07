using Base.Grid;
using Base.Math;
using UnityEngine;

namespace Base.TileMap
{
    public class TileCollectionAnalyzer : ICellTypeSource
    {
        private readonly ITileCollection _tileCollection = null;

        public TileCollectionAnalyzer(ITileCollection tileCollection)
        {
            _tileCollection = tileCollection;
        }

        public CellType GetCellTypeAt(Vector3 coordinate)
        {
            var worldCoordinate = new Vector3Iso(coordinate);
            var worldCoordinatePos = new Vector3(worldCoordinate.X, worldCoordinate.Y, worldCoordinate.Z);
            var tile = _tileCollection.GetTile(worldCoordinatePos);
            
            if (tile == null || tile.Name == null) return CellType.Air;
            
            return tile.Name switch
            {
                "Test Block Grass" => CellType.Grass,
                "Test Block Water" => CellType.Water,
                _ => CellType.Air
            };
        } 
    }
}
using Base.Grid;
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
            var tile = _tileCollection.GetTile(coordinate);
            
            if (tile == null) return CellType.Air;
            
            return tile.Name switch
            {
                "Test Block Grass" => CellType.Grass,
                "Test Block Water" => CellType.Water,
                _ => CellType.Air
            };
        } 
    }
}
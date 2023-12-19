using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.Grid
{
    public class TileMapAnalyzer
    {
        private BoundsInt _cellBounds = default;
        private TileBase[] _tiles = null;

        private TileMapAnalyzer() {}

        public TileMapAnalyzer(Tilemap tileMap)
        {
            _cellBounds = tileMap.cellBounds;
            _tiles = tileMap.GetTilesBlock(_cellBounds);
        }

        public CellType GetCellTypeAt(Vector3Int coordinate)
        {
            var tile =
                _tiles[(coordinate.x - _cellBounds.xMin) + (coordinate.y - _cellBounds.yMin) * _cellBounds.size.x];
            Debug.Log(tile.name);
            switch (tile.name)
            {
                default: return CellType.Grass;
            }
        } 
    }
}
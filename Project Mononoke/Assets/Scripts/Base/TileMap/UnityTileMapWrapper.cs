using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.TileMap
{
    public class UnityTileMapWrapper : ITileCollection
    {
        private readonly Tilemap _tileMap = null;
        private static readonly Vector3 CELL_CENTER_OFFSET = new(-0.25f, -0.25f, 0); 

        public UnityTileMapWrapper(Tilemap tileMap)
        {
            _tileMap = tileMap;
        }
        
        
        public ITile GetTile(Vector3 position)
        {
            var correctedPosition = position + CELL_CENTER_OFFSET;
            Vector3Int cellPosition = _tileMap.WorldToCell(correctedPosition);
            var tile = _tileMap.GetTile(cellPosition);
            return new UnityTileBaseWrapper(tile);
        }
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.TileMap
{
    public class UnityTileMapWrapper : ITileMapSource
    {
        private readonly Tilemap _tileMap = null;

        public UnityTileMapWrapper(Tilemap tileMap)
        {
            _tileMap = tileMap;
        }
        
        public Vector3Int WorldToCell(Vector3 position)
        {
            return _tileMap.WorldToCell(position);
        }

        public ITile GetTile(Vector3Int position)
        {
            var tile = _tileMap.GetTile(position);
            return new UnityTileBaseWrapper(tile);
        }
    }
}
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Base.TileMap
{
    public class UnityTileMapWrapper : ITileCollection
    {
        private readonly Tilemap _tileMap = null;

        public UnityTileMapWrapper(Tilemap tileMap)
        {
            _tileMap = tileMap;
        }
        
        
        public ITile GetTile(Vector3 position)
        {
            Vector3Int cellPosition = _tileMap.WorldToCell(position);
            Debug.Log($"Tilemap Cell Position {cellPosition}");
            var tile = _tileMap.GetTile(cellPosition);
            return new UnityTileBaseWrapper(tile);
        }
    }
}
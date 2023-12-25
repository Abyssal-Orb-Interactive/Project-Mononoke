using UnityEngine;

namespace Base.TileMap
{
    public interface ITileMapSource
    {
        Vector3Int WorldToCell(Vector3 coordinate);
        ITile GetTile(Vector3Int coordinate);
    }
}
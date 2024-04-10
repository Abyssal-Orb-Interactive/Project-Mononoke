using UnityEngine;

namespace Base.TileMap
{
    public interface ITileCollection
    {
        ITile GetTile(Vector3 coordinate);
    }
}
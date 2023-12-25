using UnityEngine.Tilemaps;

namespace Base.TileMap
{
    public class UnityTileBaseWrapper : ITile
    {
        private readonly TileBase _tile = null;

        public UnityTileBaseWrapper(TileBase tileBase)
        {
            _tile = tileBase;
        }
        public string Name => _tile.name;
    }
}
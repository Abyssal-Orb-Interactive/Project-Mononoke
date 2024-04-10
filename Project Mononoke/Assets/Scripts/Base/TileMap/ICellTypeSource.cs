using Base.Grid;
using UnityEngine;

namespace Base.TileMap
{
    public interface ICellTypeSource
    {
        public CellType GetCellTypeAt(Vector3 coordinate);
    }
}
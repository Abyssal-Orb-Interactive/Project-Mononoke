using Base.Grid;
using UnityEngine;

namespace Base.TileMap
{
    public class TileMapAnalyzer : ICellTypeSource
    {
        private readonly ITileMapSource _tileMap = null;

        public TileMapAnalyzer(ITileMapSource tileMap)
        {
            _tileMap = tileMap;
        }

        public CellType GetCellTypeAt(Vector3 coordinate)
        {
            var cellPosition = _tileMap.WorldToCell(coordinate);
            var tile = _tileMap.GetTile(cellPosition);
            
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
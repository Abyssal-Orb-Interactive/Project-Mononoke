using System.Collections.Generic;
using UnityEngine;

namespace Base.Grid
{
    public class GroundGrid
    {
        private Dictionary<Vector3Int, Cell> _grid = null;
        private TileMapAnalyzer _tileMapAnalyzer = null;

        private GroundGrid(){}

        public GroundGrid(TileMapAnalyzer tileMapAnalyzer)
        {
            _tileMapAnalyzer = tileMapAnalyzer;
            _grid = new Dictionary<Vector3Int, Cell>();
        }
        
        public IReadonlyCell GetCellAt(Vector3Int coordinate)
        {
            if (!_grid.ContainsKey(coordinate))
            {
                _grid.Add(coordinate, new Cell(_tileMapAnalyzer.GetCellTypeAt(coordinate)));
            }

            return _grid[coordinate];
        }
    }
}

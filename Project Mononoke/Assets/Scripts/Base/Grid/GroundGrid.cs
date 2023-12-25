using System.Collections.Generic;
using Base.TileMap;
using UnityEngine;

namespace Base.Grid
{
    public class GroundGrid
    {
        private readonly Dictionary<Vector3Int, Cell> _grid = null;
        private readonly ICellTypeSource _cellTypeSource = null;

        private GroundGrid(){}

        public GroundGrid(ICellTypeSource cellTypeSource)
        {
            _cellTypeSource = cellTypeSource;
            _grid = new Dictionary<Vector3Int, Cell>();
        }
        
        public IReadonlyCell GetCellAt(Vector3Int coordinate)
        {
            if (_grid.TryGetValue(coordinate, out var cell)) return cell;
            
            var cellType = _cellTypeSource.GetCellTypeAt(coordinate);
            cell = new Cell(cellType);
            _grid.Add(coordinate, cell);

            return cell;
        }

        public bool HasBuildingAt(Vector3Int coordinate)
        {
            return GetCellAt(coordinate).HasBuilding;
        }

        public bool IsCellPassableAt(Vector3Int coordinate)
        {
            if (HasBuildingAt(coordinate)) return false;
            
            return GetCellAt(coordinate).Type switch
            {
                CellType.Grass => true,
                _ => false
            };
        }
    }
}

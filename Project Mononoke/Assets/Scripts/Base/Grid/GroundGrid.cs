using System.Collections.Generic;
using Base.Math;
using Base.TileMap;
using Source.BuildingModule;
using Source.BuildingModule.Buildings;
using UnityEngine;

namespace Base.Grid
{
    public class GroundGrid
    {
        private readonly Dictionary<Vector3Int, Cell> _grid = null;
        private readonly ICellTypeSource _cellTypeSource = null;
        public IEnumerable<Vector3Int> CellCoordinates => _grid.Keys;
        public IEnumerable<IReadonlyCell> Cells => _grid.Values;

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
            if (HasBuildingAt(coordinate) && GetBuildingAt(coordinate) is not Seedbed) return false;

            var cell =  GetCellAt(coordinate);
             
            return cell.Type switch
            {
                CellType.Grass => true,
                _ => false
            };
        }
        
        public bool IsCellFlyableAt(Vector3Int coordinate)
        {
            if (HasBuildingAt(coordinate)) return false;

            var cell =  GetCellAt(coordinate);
             
            return cell.Type switch
            {
                CellType.Air => true,
                _ => false
            };
        }

        public Vector3Int WorldToGrid(Vector3 worldCoordinate)
        {
            var isometricCoordinate = new Vector3Iso(worldCoordinate.x, worldCoordinate.y, worldCoordinate.z);
            var cartesianCoordinate = isometricCoordinate.ToCartesian();
            var roundedCoordinate = new Vector3Int(Mathf.FloorToInt(cartesianCoordinate.x), Mathf.CeilToInt(cartesianCoordinate.y), Mathf.CeilToInt(cartesianCoordinate.z));
            return roundedCoordinate;
        }

        public bool TryAddBuilding(Building building, Vector3Int position)
        {
            var cell = _grid[position];

            if(cell.HasBuilding) return false;

            cell.AddBuilding(building);
            return true;
        }

        public Building GetBuildingAt(Vector3Int position)
        {
            var cell = _grid[position];
            return cell.Building;
        }
    }
}

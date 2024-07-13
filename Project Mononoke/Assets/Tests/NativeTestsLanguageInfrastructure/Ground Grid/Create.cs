using Base.Grid;
using Base.TileMap;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.Ground_Grid
{
    public static partial class Create
    {
        public static Cell ExpectedCell()
        {
            return new Cell(TestParameter.ExpectedCellType);
        }
        
        public static GroundGrid GridWith(ICellTypeSource cellTypeSource)
        {
            return new GroundGrid(cellTypeSource);
        }
        
        public static Vector3Int Vector3IntWith(int x, int y, int z)
        {
            return new Vector3Int(x,y,z);
        }
    }
}
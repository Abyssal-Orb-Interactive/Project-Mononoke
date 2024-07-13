using Base.Grid;
using Base.TileMap;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.Ground_Grid
{
    public static partial class TestParameter
    {
        public static IReadonlyCell ExpectedCell = null;
        public static Vector3Int AssumedCellCoordinate = Vector3Int.zero;
        public static ICellTypeSource TileMapAnalyzerSubstitute = null;
        public static GroundGrid Grid = null;
        public static CellType ExpectedCellType = CellType.Air;
    }
}
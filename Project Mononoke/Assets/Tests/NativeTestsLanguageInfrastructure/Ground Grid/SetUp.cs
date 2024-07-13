using Base.Grid;
using Base.TileMap;
using NSubstitute;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure.Ground_Grid
{
    public static partial class SetUp
    {
        public static void ExpectedCellWith(CellType type)
        {
            TestParameter.ExpectedCellType = type;
            TestParameter.ExpectedCell = Create.ExpectedCell();
        }
        
        public static void TileMapAnalyzerSubstitute()
        { 
            TestParameter.TileMapAnalyzerSubstitute = Substitute.For<ICellTypeSource>();
            TestParameter.TileMapAnalyzerSubstitute.GetCellTypeAt(Arg.Any<Vector3>()).Returns(TestParameter.ExpectedCellType);
        }
        
        public static void AssumedCellCoordinateWith(int x, int y, int z)
        {
            TestParameter.AssumedCellCoordinate = Create.Vector3IntWith(x, y, z);
        }
        
        public static void Grid()
        {
            TileMapAnalyzerSubstitute();
            TestParameter.Grid = Create.GridWith(TestParameter.TileMapAnalyzerSubstitute);
        }
    }
}
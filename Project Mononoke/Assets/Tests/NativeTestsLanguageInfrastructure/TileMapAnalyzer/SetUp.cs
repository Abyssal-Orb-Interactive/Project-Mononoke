using Base.Grid;
using Base.TileMap;
using NSubstitute;
using UnityEngine;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class SetUp
    {
        public static void AssumedCellPositionAndExpectedCellTypeWith(int x, int y, int z, CellType type)
        {
            TestParameter.CellPosition = Create.Vector3With(x,y,z);
            TestParameter.ExpectedCellType = type;
        }
        
        public static void TilePositionAndTileWith(int x, int y, int z, ITile tile)
        {
            TestParameter.TilePosition = Create.Vector3IntWith(x, y, z);
            TestParameter.TileSubstitute = tile;
        }
        
        public static void TileNameWith(string name)
        {
            TestParameter.TileName = name;
        }
        
        public static void TileSubstituteWith(string name)
        {
            TileNameWith(name);
            TestParameter.TileSubstitute = Substitute.For<ITile>();
            TestParameter.TileSubstitute.Name.Returns(TestParameter.TileName);
        }
        
        public static void TileMapSubstitute()
        {
            TestParameter.TileMapSubstitute = Create.TileMapSubstitute();
            TestParameter.TileMapSubstitute.WorldToCell(Arg.Any<Vector3>()).Returns(TestParameter.TilePosition);
            TestParameter.TileMapSubstitute.GetTile(Arg.Any<Vector3Int>()).Returns(TestParameter.TileSubstitute);
        }

        public static void TilePositionAndTileNameWith(int x, int y, int z, string name)
        {
            TileSubstituteWith(name);
            TilePositionAndTileWith(x, y, z, TestParameter.TileSubstitute);
        }
        
        public static void TileMapAnalyzer()
        {
            TileMapSubstitute();
            TestParameter.TileMapAnalyzer = Create.TileMapAnalyzerWith(TestParameter.TileMapSubstitute);
        }
    }
}
using Base.Grid;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.TileMapAnalyzerTests
{
    public class GetCellTypeAtTests
    {
        [Test]
        public void WhenTileIsMissingAt111_AndGetCellTypeAt111_ThenCellTypeShouldBeAir()
        {
            // Arrange.
            SetUp.AssumedCellPositionAndExpectedCellTypeWith(1, 1, 1, CellType.Air);
            SetUp.TilePositionAndTileWith(1, 1, 1, null);
            SetUp.TileMapAnalyzer();
            // Act.
            var result = TestParameter.TileMapAnalyzer.GetCellTypeAt(TestParameter.CellPosition);
            // Assert.
            Check.IsExpectedCellTypeMatchesWith(result);
        }

        [Test]
        public void WhenTileIsGrassAt123_AndGetCellTypeAt123_ThenCellTypeShouldBeGrass()
        {
            // Arrange.
            SetUp.AssumedCellPositionAndExpectedCellTypeWith(1, 2, 3, CellType.Grass);
            SetUp.TilePositionAndTileNameWith(1,2,3, "Test Block Grass");
            SetUp.TileMapAnalyzer();
            // Act.
            var result = TestParameter.TileMapAnalyzer.GetCellTypeAt(TestParameter.CellPosition);
            // Assert.
            Check.IsExpectedCellTypeMatchesWith(result);
        }

        [Test]
        public void WhenTileIsWaterAtMinus101_AndGetCellTypeAtMinus101_ThenCellTypeShouldBeWater()
        {
            // Arrange.
            SetUp.AssumedCellPositionAndExpectedCellTypeWith(-1, 0, -1, CellType.Water);
            SetUp.TilePositionAndTileNameWith(-1,0,-1, "Test Block Water");
            SetUp.TileMapAnalyzer();
            // Act.
            var result = TestParameter.TileMapAnalyzer.GetCellTypeAt(TestParameter.CellPosition);
            // Assert.
            Check.IsExpectedCellTypeMatchesWith(result);
        }
    }
}
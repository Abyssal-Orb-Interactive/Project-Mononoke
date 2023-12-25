using Base.Grid;
using Base.TileMap;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;
using UnityEngine;

namespace Tests.Grid_Tests.Ground_Grid_Tests
{
    public class GetCellAtTests
    {
        [Test]
        public void WhenGroundGridDoesNotContainsCellAt000_AndGetCellAt000_ThenGridShouldCreateCellUsingTileAnalyzerReturnedTileTypeGrass()
        {
            // Arrange.
            SetUp.ExpectedCellWith(CellType.Grass);
            SetUp.AssumedCellCoordinateWith(0, 0, 0);
            SetUp.Grid();
            // Act.
            var result = TestParameter.Grid.GetCellAt(TestParameter.AssumedCellCoordinate);
            // Assert.
            Check.IsExpectedCellEquivalentTo(result);
        }
    }
}
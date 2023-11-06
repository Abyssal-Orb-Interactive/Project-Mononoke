using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.GridTests
{
    public class GridConstructTests
    {
        [Test]
        public void WhenGridConstructing_AndSizeIs0_0_ThenSizeShouldBeGridConstructionException()
        {
            // Arrange.
            SetUp.WrongSizeWith(0,0);
            // Act.
            Try.CatchConstructionExceptionWhileConstructingGridWith(TestParameter.Size);
            // Assert.
            Check.IsThereAnGridConstructionError();
        }

        [Test]
        public void WhenGridConstructing_AndSizeIsNegative1_1_ThenShouldBeGridConstructionException()
        {
            // Arrange.
            SetUp.WrongSizeWith(-1,-1);
            // Act.
            Try.CatchConstructionExceptionWhileConstructingGridWith(TestParameter.Size);
            // Assert.
            Check.IsThereAnGridConstructionError();
        }
        
        [Test]
        public void WhenGridConstructing_AndSizeIsNegative1_Positive1_ThenShouldBeGridConstructionException()
        {
            // Arrange.
            SetUp.WrongSizeWith(-1, 1);
            // Act.
            Try.CatchConstructionExceptionWhileConstructingGridWith(TestParameter.Size);
            // Assert.
            Check.IsThereAnGridConstructionError();
        }
        
        [Test]
        public void WhenGridConstructing_AndSizeIsPositive1_Negative1_ThenShouldBeGridConstructionException()
        {
            // Arrange.
            SetUp.WrongSizeWith(1, -1);
            // Act.
            Try.CatchConstructionExceptionWhileConstructingGridWith(TestParameter.Size);
            // Assert.
            Check.IsThereAnGridConstructionError();
        }
        
        [Test]
        public void WhenGridConstructing_AndCellSizeIsNegative_ThenShouldBeGridConstructionException()
        {
            // Arrange.
            SetUp.CellSizeWithError(-1f);
            // Act.
            Try.CatchConstructionExceptionWhileConstructingGridWith(TestParameter.Size, TestParameter.CellSize);
            // Assert.
            Check.IsThereAnGridConstructionError();
        }
        
        [Test]
        public void WhenGridConstructing_AndCellSizeIsZero_ThenShouldBeGridConstructionException()
        {
            // Arrange.
            SetUp.CellSizeWithError(0f);
            // Act.
            Try.CatchConstructionExceptionWhileConstructingGridWith(TestParameter.Size, TestParameter.CellSize);
            // Assert.
            Check.IsThereAnGridConstructionError();
        }
    }
}

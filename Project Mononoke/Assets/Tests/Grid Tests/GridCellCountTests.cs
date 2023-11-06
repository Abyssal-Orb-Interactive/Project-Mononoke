using NUnit.Framework;
using Tests.NativeTestsLanguageInfrastructure;

namespace Tests.GridTests
{
    public class GridCellCountTests
    {
        [Test]
        public void WhenGridCreated_AndSizeIs10_10_ThenCellCountShouldBe100()
        {
            // Arrange.
            SetUp.SizeAndCountPatternWith(10,10,100);
            // Act.
            SetUp.GridWith(TestParameter.Size);
            // Assert.
            Check.IsCellCountMatchesWith(TestParameter.CountPattern);
        }
    }
}

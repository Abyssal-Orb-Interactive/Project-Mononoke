using Base.Grid;
using FluentAssertions;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void IsExpectedCellTypeMatchesWith(CellType type)
        {
            new { CellType = type }.Should().Be(new { CellType = TestParameter.ExpectedCellType });
        }
    }
}
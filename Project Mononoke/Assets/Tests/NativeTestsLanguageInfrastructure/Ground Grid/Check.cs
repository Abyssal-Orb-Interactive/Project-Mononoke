using Base.Grid;
using FluentAssertions;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void IsExpectedCellEquivalentTo(IReadonlyCell result)
        {
            new { Cell = result }.Should().BeEquivalentTo(new { Cell = TestParameter.ExpectedCell });
        }
    }
}
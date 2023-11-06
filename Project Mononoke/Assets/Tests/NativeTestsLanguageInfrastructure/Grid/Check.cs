using Base.Grid;
using FluentAssertions;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Check
    {
        public static void IsThereAnGridConstructionError()
        {
            new { Exception = TestParameter.ConstructionException }.Should().NotBe(new { Exception = (GridConstructionException) null });
        }

        public static void IsCellCountMatchesWith(int countPattern)
        {
            new {CellCount = TestParameter.Grid.CellCount}.Should().Be(new {CellCount = countPattern});
        }
    }
}
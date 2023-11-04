using Base.Grid;
using Base.Math;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class SetUp
    {
        public static void SizeWith(int x , int y)
        {
            TestParameter.Size = Create.InPlaneCoordinateInt(x, y);
        }

        public static void GridWith(InPlaneCoordinateInt size)
        {
            TestParameter.Grid = Create.Grid(size);
        }

        public static void WrongSizeWith(int x, int y)
        {
            SizeWith(x, y);
            TestParameter.ConstructionException = null;
        }

        public static void ConstructionExceptionWith(GridConstructionException exception)
        {
            TestParameter.ConstructionException = exception;
        }
        
        public static void SizeAndCountPatternWith(int x, int y, int countPattern)
        {
            SizeWith(x, y);
            CountPatternWith(countPattern);
        }
    }
}
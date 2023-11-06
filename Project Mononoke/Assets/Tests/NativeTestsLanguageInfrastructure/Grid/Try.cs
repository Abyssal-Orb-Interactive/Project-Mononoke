using Base.Grid;
using Base.Math;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Try
    {
        public static void CatchConstructionExceptionWhileConstructingGridWith(InPlaneCoordinateInt size, float cellSize = 10f)
        {
            try
            {
                Create.Grid(size, cellSize);
            }
            catch (GridConstructionException e)
            {
                SetUp.ConstructionExceptionWith(e);
            }
        }
    }
}
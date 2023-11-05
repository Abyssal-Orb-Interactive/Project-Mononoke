using Base.Grid;
using Base.Math;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Try
    {
        public static void CatchConstructionExceptionWhileConstructingGridWith(InPlaneCoordinateInt size)
        {
            try
            {
                Create.Grid(size);
            }
            catch (GridConstructionException e)
            {
                SetUp.ConstructionExceptionWith(e);
            }
        }
    }
}
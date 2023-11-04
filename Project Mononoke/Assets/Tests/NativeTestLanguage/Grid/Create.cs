using Base.Grid;
using Base.Math;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Create
    {
        public static InPlaneCoordinateInt InPlaneCoordinateInt(int x, int y)
        {
            return new InPlaneCoordinateInt(x, y);
        }

        public static Grid Grid(InPlaneCoordinateInt size)
        {
            return new Grid(size);
        }
    }
}
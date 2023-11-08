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

        public static Grid<int> Grid(InPlaneCoordinateInt size, float cellSize = 10f)
        {
            return new Grid<int>(size, () => 0 ,cellArea: cellSize);
        }
    }
}
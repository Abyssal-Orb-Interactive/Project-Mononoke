using Base.Grid;
using Base.TileMap;

namespace Tests.NativeTestsLanguageInfrastructure
{
    public static partial class Create
    {
        public static Cell ExpectedCell()
        {
            return new Cell(TestParameter.ExpectedCellType);
        }
        
        public static GroundGrid GridWith(ICellTypeSource cellTypeSource)
        {
            return new GroundGrid(cellTypeSource);
        }
    }
}
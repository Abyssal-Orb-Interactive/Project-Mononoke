namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        public bool HasBuilding { get; private set; } = false;
        public CellType Type { get; private set; } = CellType.Air;

        public Cell(CellType type)
        {
            Type = type;
        }
    }
}

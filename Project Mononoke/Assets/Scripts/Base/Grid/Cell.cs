namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        public bool HasBuilding { get; private set; }
        public CellType Type { get; private set; }

        public Cell(CellType type)
        {
            Type = type;
        }
    }
}

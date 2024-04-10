namespace Base.Grid
{
    public interface IReadonlyCell
    {
        public bool HasBuilding { get;}
        public CellType Type { get;}
    }
}
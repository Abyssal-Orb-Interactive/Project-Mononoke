using Source.BuildingModule;

namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        public Building Building { get; private set; }
        public bool HasBuilding => Building != null;
        public CellType Type { get; private set; } = CellType.Air;

        public Cell(CellType type)
        {
            Type = type;
        }
        
        public void AddBuilding(Building building)
        {
            Building = building;
        }
    }
}

using Source.BuildingModule.Buildings;

namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        private Seedbed _building = null;
        public bool HasBuilding => _building != null;
        public CellType Type { get; private set; } = CellType.Air;

        public Cell(CellType type)
        {
            Type = type;
        }
        
        public void AddBuilding(Seedbed building)
        {
            _building = building;
        }
    }
}

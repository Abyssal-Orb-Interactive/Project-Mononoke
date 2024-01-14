using UnityEngine;

namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        private GameObject _building = null;
        public bool HasBuilding { get {return _building != null;}}
        public CellType Type { get; private set; } = CellType.Air;

        public Cell(CellType type)
        {
            Type = type;
        }
        
        public void AddBuilding(GameObject building)
        {
            _building = building;
        }
    }
}

using UnityEngine;

namespace Base.Grid
{
    public class Cell : IReadonlyCell
    {
        public Vector3 Sizes => new(1f, 0.5f, 1f);
        public bool HasBuilding { get; private set; } = false;
        public CellType Type { get; private set; } = CellType.Air;

        public Cell(CellType type)
        {
            Type = type;
        }
    }
}

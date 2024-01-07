using UnityEngine;

namespace Base.Grid
{
    public interface IReadonlyCell
    {
        public Vector3 Sizes {get;}
        public bool HasBuilding { get;}
        public CellType Type { get;}
    }
}
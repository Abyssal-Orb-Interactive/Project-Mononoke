using Base.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.UI
{
    public class SelectedCellIndicatorMover : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        
        private GameObject _indicator = null;
        private Mouse _cursor = null;

        private void OnValidate()
        {
            _indicator ??= gameObject;
            _cursor ??= Mouse.current;
        }

        public void Update()
        {
            var gridPosition = _grid.WorldToCell(_cursor.GetMouseWorldPosition());
            _indicator.transform.position = _grid.CellToWorld(gridPosition);
        }
    }
}

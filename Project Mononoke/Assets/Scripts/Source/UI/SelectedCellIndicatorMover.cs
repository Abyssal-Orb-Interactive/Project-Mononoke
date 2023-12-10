using Base.Utils;
using UnityEngine;

namespace Source.UI
{
    public class SelectedCellIndicatorMover : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        private GameObject _indicator = null;

        public void Update()
        {
            _indicator ??= gameObject;
            var gridPosition = _grid.WorldToCell(MouseUtils.GetMouseWorldPosWithoutZUsingNewInputSystem());
            _indicator.transform.position = _grid.CellToWorld(gridPosition);
        }
    }
}

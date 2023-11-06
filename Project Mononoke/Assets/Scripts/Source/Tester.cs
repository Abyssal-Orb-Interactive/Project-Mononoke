using Base.Grid;
using Base.Math;
using Base.Utils;
using UnityEngine;
using Grid = Base.Grid.Grid;

namespace Source
{
    [RequireComponent(typeof(GridVisualizer))]
    public class Tester : MonoBehaviour
    {
        [SerializeField] private GridVisualizer _gridVisualizer;

        private void OnValidate()
        {
            _gridVisualizer ??= GetComponent<GridVisualizer>();
        }

        private void Start()
        {
            _gridVisualizer.Visualize(new Grid(new InPlaneCoordinateInt(4, 4)));
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = MouseUtils.GetMouseWorldPosWithoutZ();
                var gridValue = _gridVisualizer.GetCellValue(mousePos);
                _gridVisualizer.SetCellValue(mousePos, gridValue + 5);
            }
            if(Input.GetMouseButtonDown(1)) Debug.Log( _gridVisualizer.GetCellValue(MouseUtils.GetMouseWorldPosWithoutZ()));
            
        }
    }
}
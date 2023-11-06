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
            if(Input.GetMouseButtonDown(0)) _gridVisualizer.SetCellValue(MouseUtils.GetMouseWorldPosWithoutZ(), 56);
            if(Input.GetMouseButtonDown(1)) Debug.Log( _gridVisualizer.GetCellValue(MouseUtils.GetMouseWorldPosWithoutZ()));
            
        }
    }
}
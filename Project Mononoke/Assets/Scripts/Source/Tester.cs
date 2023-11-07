using Base.Grid;
using Base.Math;
using Base.Utils;
using UnityEngine;
using Grid = Base.Grid.Grid<bool>;

namespace Source
{
    [RequireComponent(typeof(GridVisualizer))]
    [RequireComponent(typeof(IntHeatMapVisualizer))]
    [RequireComponent(typeof(BoolHeatMapVisualizer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Tester : MonoBehaviour
    {
        [SerializeField] private GridVisualizer _gridVisualizer;
        [SerializeField] private IntHeatMapVisualizer _intHeatMapVisualizer;
        [SerializeField] private BoolHeatMapVisualizer _boolHeatMapVisualizer;
        [SerializeField] private MeshFilter _meshFilter;

        private Grid _grid;
        private void Start()
        {
            _grid = new Grid(new InPlaneCoordinateInt(20, 10));
            _gridVisualizer.Visualize(_grid);
            //intHeatMapVisualizer.Initialize(_grid, _meshFilter);
            _boolHeatMapVisualizer.Initialize(_grid, _meshFilter);
        }

        private void OnValidate()
        {
            _gridVisualizer ??= GetComponent<GridVisualizer>();
            _intHeatMapVisualizer ??= GetComponent<IntHeatMapVisualizer>();
            _meshFilter ??= GetComponent<MeshFilter>();
            _boolHeatMapVisualizer ??= GetComponent<BoolHeatMapVisualizer>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = MouseUtils.GetMouseWorldPosWithoutZ();
                var mouseInGridPos = GridPositionConverter.GetCoordinateInGrid(mousePos, _grid.CellSize, _grid.OriginPosition);
                _grid.TrySetValue(mouseInGridPos, true);
            }
        }

        private void LateUpdate()
        {
            _intHeatMapVisualizer.UpdateInTheEndOfFrame();
            _boolHeatMapVisualizer.UpdateInTheEndOfFrame();
        }
    }
}
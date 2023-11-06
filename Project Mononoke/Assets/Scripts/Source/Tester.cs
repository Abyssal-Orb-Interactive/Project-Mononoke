using Base.Grid;
using Base.Math;
using Base.Utils;
using UnityEngine;
using Grid = Base.Grid.Grid;

namespace Source
{
    [RequireComponent(typeof(GridVisualizer))]
    [RequireComponent(typeof(HeatMapVisualizer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Tester : MonoBehaviour
    {
        [SerializeField] private GridVisualizer _gridVisualizer;
        [SerializeField] private HeatMapVisualizer _heatMapVisualizer;
        [SerializeField] private MeshFilter _meshFilter;

        private Grid _grid;
        private void Start()
        {
            _grid = new Grid(new InPlaneCoordinateInt(100, 100), cellSize: 4f);
            _gridVisualizer.Visualize(_grid);
            _heatMapVisualizer.Initialize(_grid, _meshFilter);
        }

        private void OnValidate()
        {
            _gridVisualizer ??= GetComponent<GridVisualizer>();
            _heatMapVisualizer ??= GetComponent<HeatMapVisualizer>();
            _meshFilter ??= GetComponent<MeshFilter>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mousePos = MouseUtils.GetMouseWorldPosWithoutZ();
                var mouseInGridPos = GridPositionConverter.GetCoordinateInGrid(mousePos, _grid);
                _grid.AddValueToCells(mouseInGridPos, 100, 5, 40);
            }
        }

        private void LateUpdate()
        {
            _heatMapVisualizer.UpdateInTheEndOfFrame();
        }
    }
}
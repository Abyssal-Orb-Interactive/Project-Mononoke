using Base.Grid;
using Base.Math;
using Base.Utils;
using UnityEngine;
using Grid = Base.Grid.Grid<bool>;

namespace Source
{
    [RequireComponent(typeof(GridVisualizer))]
    [RequireComponent(typeof(HeatMapVisualizer))]
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Tester : MonoBehaviour
    {
        [SerializeField] private GridVisualizer _gridVisualizer;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private HeatMapVisualizer _heatMapVisualizer;

        private PathFinder _pathFinder;
        private void Start()
        {
            _pathFinder = new PathFinder(new InPlaneCoordinateInt(10, 10));
            _gridVisualizer.Visualize(_pathFinder.GetGrid());
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
                var mouseInGridPos = GridPositionConverter.GetCoordinateInGrid(mousePos, _pathFinder.GetGrid().CellArea, _pathFinder.GetGrid().OriginPosition);
            }
        }

        private void LateUpdate()
        {
        }
    }
}
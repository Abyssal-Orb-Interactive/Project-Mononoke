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
                var path = _pathFinder.FindPath(new InPlaneCoordinateInt(0, 0), mouseInGridPos);
                if (path != null)
                {
                    for (var pathNodeIndex = 0; pathNodeIndex < path.Count; pathNodeIndex++)
                    {
                        var coordinate = _pathFinder.GetGrid().GetCoordinateOfCell(path[pathNodeIndex]);
                        if(pathNodeIndex + 1 >= path.Count) return;
                        var nextNodeCoordinate = _pathFinder.GetGrid().GetCoordinateOfCell(path[pathNodeIndex + 1]);
                        Debug.DrawLine(new Vector3(coordinate.X, coordinate.Y) * 10f + Vector3.one * 5f, new Vector3(nextNodeCoordinate.X, nextNodeCoordinate.Y) * 10f + Vector3.one * 5f, Color.green, 100f);
                    }
                }
            }
        }

        private void LateUpdate()
        {
        }
    }
}
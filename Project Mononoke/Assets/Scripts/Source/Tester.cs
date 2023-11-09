using Base.Grid;
using Base.Grid.Pathfinder;
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
        [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
        [SerializeField] private PathfindingVisual pathfindingVisual;
        [SerializeField] private GridVisualizer _gridVisualizer;

        private Pathfinder _pathfinder;
        private void Start()
        {
            _pathfinder = new Pathfinder(new InPlaneCoordinateInt(20, 10));
            pathfindingDebugStepVisual.SetUp(_pathfinder.GetGrid());
            pathfindingVisual.Initialize(_pathfinder.GetGrid());
            _gridVisualizer.Visualize(_pathfinder.GetGrid());
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var mouseWorldPosition = MouseUtils.GetMouseWorldPosWithoutZ();
                var mouseInGridPos = GridPositionConverter.GetCoordinateInGrid(mouseWorldPosition, _pathfinder.GetGrid().CellArea, _pathfinder.GetGrid().OriginPosition);
                var path = _pathfinder.FindPath(new InPlaneCoordinateInt(0, 0), mouseInGridPos);
                if (path != null)
                {
                    for (var pathNodeIndex = 0; pathNodeIndex < path.Count; pathNodeIndex++)
                    {
                        var coordinate = _pathfinder.GetGrid().GetCoordinateOfCell(path[pathNodeIndex]);
                        if(pathNodeIndex + 1 >= path.Count) return;
                        var nextNodeCoordinate = _pathfinder.GetGrid().GetCoordinateOfCell(path[pathNodeIndex + 1]);
                        Debug.DrawLine(new Vector3(coordinate.X, coordinate.Y) * 10f + Vector3.one * 5f, new Vector3(nextNodeCoordinate.X, nextNodeCoordinate.Y) * 10f + Vector3.one * 5f, Color.green, 100f);
                    }
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                var mouseWorldPosition = MouseUtils.GetMouseWorldPosition();
                var mouseInGridPos = GridPositionConverter.GetCoordinateInGrid(mouseWorldPosition, _pathfinder.GetGrid().CellArea, _pathfinder.GetGrid().OriginPosition);
                _pathfinder.GetNode(mouseInGridPos).IsWalkable = !_pathfinder.GetNode(mouseInGridPos).IsWalkable;
            }
            
            pathfindingDebugStepVisual.UpdatedVisual();
        }

        private void LateUpdate()
        {
            pathfindingVisual.UpdateInTheEndOfFrame();
        }
    }
}
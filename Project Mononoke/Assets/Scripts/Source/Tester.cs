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
        }

        private void LateUpdate()
        {
            pathfindingVisual.UpdateInTheEndOfFrame();
        }
    }
}
using Base.Grid;
using Base.Math;
using UnityEngine;
using Grid = Base.Grid.Grid;

namespace Source
{
    [RequireComponent(typeof(GridVisualizer))]
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridVisualizer _gridVisualizer;

        private void OnValidate()
        {
            _gridVisualizer ??= GetComponent<GridVisualizer>();
        }

        private void Start()
        {
            _gridVisualizer.Visualize(new Grid(new InPlaneCoordinateInt(10, 10)));
        }
    }
}
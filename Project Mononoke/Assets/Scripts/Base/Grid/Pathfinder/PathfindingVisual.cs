using Base.Grid.CellContent;
using Base.Math;
using Base.MeshBuilder;
using UnityEngine;

namespace Base.Grid.Pathfinder
{
    [RequireComponent(typeof(MeshFilter))]
    public class PathfindingVisual : MonoBehaviour
    {
        private Grid<PathNode> _grid;
        private Mesh _mesh;
        private bool _updateMeshDesired;

        private void InitializeMesh()
        {
            _mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _mesh;
        }

        public void Initialize(Grid<PathNode> grid)
        {
            InitializeMesh();
            _grid = grid;
            UpdateVisual();
            
            _grid.SubscribeOnCellValueChanged(OnGridValueChanged);
        }

        private void OnGridValueChanged(object sender, Grid<PathNode>.OnGridValueChangedEventArgs args)
        {
            _updateMeshDesired = true;
        }

        public void UpdateInTheEndOfFrame()
        {
            if (!_updateMeshDesired) return;
            
            _updateMeshDesired = false;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            var properties = MeshPropertiesFabric.CreateEmptyGridMeshProperties(_grid.CellCount);

            for (var x = 0; x < _grid.Sizes.X; x++)
            {
                for (var y = 0; y < _grid.Sizes.Y; y++)
                {
                    var cellIndex = x * _grid.Sizes.Y + y;
                    var cellVectorSize = Vector3.one * _grid.CellArea;
                    var cellWorldPosition = GridPositionConverter.GetWorldPosition(new InPlaneCoordinateInt(x, y), _grid.CellArea, _grid.OriginPosition) + cellVectorSize * 0.5f;

                    var pathNode = _grid.GetCellValue(new InPlaneCoordinateInt(x, y));

                    if (pathNode.IsWalkable)
                    {
                        cellVectorSize = Vector3.zero;
                    }
                    
                    MeshPropertiesFabric.AddToMeshArrays(properties, cellIndex, cellWorldPosition, 0f ,cellVectorSize, Vector2.zero, Vector2.zero);
                }
            }

            _mesh.vertices = properties.Vertices;
            _mesh.uv = properties.UV;
            _mesh.triangles = properties.Triangles;
        }
    }
}
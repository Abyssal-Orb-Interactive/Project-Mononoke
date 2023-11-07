using Base.Math;
using Base.MeshBuilder;
using UnityEngine;

namespace Base.Grid
{
    public class BoolHeatMapVisualizer : MonoBehaviour
    {
        private Grid<bool> _grid;
        private Mesh _mesh;
        private bool _updatedDesired;

        public void Initialize(Grid<bool> grid, MeshFilter meshFilter)
        {
            _grid = grid ?? new Grid<bool>(new InPlaneCoordinateInt(1,1));
            _mesh = new Mesh();
            meshFilter.mesh = _mesh;
            UpdateMapVisual();
            
            _grid.SubscribeOnCellValueChanged(OnGridValueChanged);
        }

        private void OnGridValueChanged(object sender, Grid<bool>.OnGridValueChangedEventArgs e)
        {
            _updatedDesired = true;
        }

        private void UpdateMapVisual()
        {
            var properties = MeshPropertiesFabric.CreateEmptyGridMeshProperties(_grid.CellCount);

            for (var x = 0; x < _grid.Sizes.X; x++)
            {
                for (var y = 0; y < _grid.Sizes.Y; y++)
                {
                    var cellIndex = x * _grid.Sizes.Y + y;
                    var cellVectorSize = Vector3.one * _grid.CellSize;
                    var cellWorldPosition = GridPositionConverter.GetWorldPosition(new InPlaneCoordinateInt(x, y), _grid.CellSize, _grid.OriginPosition) + cellVectorSize * 0.5f;

                    var gridValue = _grid.GetCellValue(new InPlaneCoordinateInt(x, y));
                    var normalizedGridValue = gridValue ? 1f : 0f;
                    var gridValueUV = new Vector2(normalizedGridValue, 0);
                    
                    MeshPropertiesFabric.AddToMeshArrays(properties, cellIndex, cellWorldPosition, 0f, cellVectorSize, gridValueUV, gridValueUV);
                }
            }

            _mesh.vertices = properties.Vertices;
            _mesh.uv = properties.UV;
            _mesh.triangles = properties.Triangles;
        }

        public void UpdateInTheEndOfFrame()
        {
            if(!_updatedDesired) return;

            _updatedDesired = false;
            UpdateMapVisual();
            
        }
    }
}
using UnityEngine;

namespace Base.MeshBuilder
{
    public static class MeshFabric
    {
        public static Mesh CreateMesh(MeshProperties properties)
        {
            var mesh = new Mesh
            {
                vertices = properties.Vertices,
                uv = properties.UV,
                triangles = properties.Triangles
            };

            return mesh;
        }
    }
}
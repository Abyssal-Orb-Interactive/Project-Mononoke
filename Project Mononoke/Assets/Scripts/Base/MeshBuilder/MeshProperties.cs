using UnityEngine;

namespace Base.MeshBuilder
{
    public record MeshProperties(Vector3[] Vertices, Vector2[] UV, int[] Triangles)
    {
        public Vector3[] Vertices { get; } = Vertices;
        public Vector2[] UV { get; } = UV;
        public int[] Triangles { get; } = Triangles;
    }
}
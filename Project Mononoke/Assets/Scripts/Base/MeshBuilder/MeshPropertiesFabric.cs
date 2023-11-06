using Base.Math;
using JetBrains.Annotations;
using UnityEngine;

namespace Base.MeshBuilder
{
    public static class MeshPropertiesFabric
    {
        public static MeshProperties CreateBasicTriangleMeshProperties()
        {
            return new MeshProperties(ConfigureVerticesForTriangle(), ConfigureUVForTriangle(),
                ConfigureTrianglesForTriangle());
        }

        private static int[] ConfigureTrianglesForTriangle()
        {
            var triangles = new int[3];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;
            return triangles;
        }

        private static Vector2[] ConfigureUVForTriangle()
        {
            var uv = new Vector2[3];
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            return uv;
        }

        private static Vector3[] ConfigureVerticesForTriangle()
        {
            var vertices = new Vector3[3];
            vertices[0] = new Vector3(0,0);
            vertices[1] = new Vector3(0,100);
            vertices[2] = new Vector3(100,100);
            return vertices;
        }
        public static MeshProperties CreateBasicQuadMeshProperties()
        {
            return new MeshProperties(ConfigureVerticesForQuad(), ConfigureUVForQuad(),
                ConfigureTrianglesForQuad());
        }
        
        private static int[] ConfigureTrianglesForQuad()
        {
            var triangles = new int[6];
            triangles[0] = 0;
            triangles[1] = 1;
            triangles[2] = 2;

            triangles[3] = 0;
            triangles[4] = 2;
            triangles[5] = 3;
            return triangles;
        }
        
        private static Vector2[] ConfigureUVForQuad()
        {
            var uv = new Vector2[4];
            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);
            return uv;
        }
        
        private static Vector3[] ConfigureVerticesForQuad()
        {
            var vertices = new Vector3[4];
            vertices[0] = new Vector3(0,0);
            vertices[1] = new Vector3(0,100);
            vertices[2] = new Vector3(100,100);
            vertices[3] = new Vector3(100,   0);
            return vertices;
        }

        public static MeshProperties CreateTileMapMeshProperties([CanBeNull] InPlaneCoordinateInt mapSizes = null, float tileSize = 10f)
        {
            const int UV_AND_VERTICES_QUAD_SIZE = 4;
            const int TRIANGLES_QOUAD_SIZE = 6;

            mapSizes ??= new InPlaneCoordinateInt(10, 10);
            
            var width = mapSizes.X;
            var height = mapSizes.Y;
            var area = width * height;

            var vertices = new Vector3[UV_AND_VERTICES_QUAD_SIZE * area];
            var uv = new Vector2[UV_AND_VERTICES_QUAD_SIZE * area];
            var triangles = new int[TRIANGLES_QOUAD_SIZE * area];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var index = x * height + y;
                    var inTileOffset = 0;

                    vertices[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector3(tileSize * x, tileSize * y);
                    uv[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector2(0, 0);
                    triangles[index * TRIANGLES_QOUAD_SIZE + inTileOffset] = index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset;
                    inTileOffset++;
                    vertices[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector3(tileSize * x, tileSize * (y + 1));
                    uv[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector2(0, 1);
                    triangles[index * TRIANGLES_QOUAD_SIZE + inTileOffset] = index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset;
                    inTileOffset++;
                    vertices[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector3(tileSize * (x + 1), tileSize * (y + 1));
                    uv[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector2(1, 1);
                    triangles[index * TRIANGLES_QOUAD_SIZE + inTileOffset] = index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset;
                    inTileOffset++;
                    vertices[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector3(tileSize * (x + 1), tileSize * y);
                    uv[index * UV_AND_VERTICES_QUAD_SIZE + inTileOffset] = new Vector2(1, 0);
                    triangles[index * TRIANGLES_QOUAD_SIZE + inTileOffset] = index * UV_AND_VERTICES_QUAD_SIZE + 0;
                    inTileOffset++;
                    triangles[index * TRIANGLES_QOUAD_SIZE + inTileOffset] = index * UV_AND_VERTICES_QUAD_SIZE + 2;
                    inTileOffset++;
                    triangles[index * TRIANGLES_QOUAD_SIZE + inTileOffset] = index * UV_AND_VERTICES_QUAD_SIZE + 3;
                } 
            }

            return new MeshProperties(vertices, uv, triangles);
        }
    }
}
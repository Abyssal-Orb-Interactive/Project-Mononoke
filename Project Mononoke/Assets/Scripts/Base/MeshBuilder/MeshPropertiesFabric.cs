using Base.Math;
using JetBrains.Annotations;
using UnityEngine;

namespace Base.MeshBuilder
{
    public static class MeshPropertiesFabric
    {
        const int VERTECES_AND_UVS_IN_QUAD = 4;
        const int TRIANGLES_IN_QUAD = 6;
        const int MAX_ANGLE_IN_DEGREESE = 360;

        private static Quaternion[] _quaternionEulerCache;
        
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
            mapSizes ??= new InPlaneCoordinateInt(10, 10);
            
            var width = mapSizes.X;
            var height = mapSizes.Y;
            var area = width * height;

            var vertices = new Vector3[VERTECES_AND_UVS_IN_QUAD * area];
            var uv = new Vector2[VERTECES_AND_UVS_IN_QUAD * area];
            var triangles = new int[TRIANGLES_IN_QUAD * area];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var index = x * height + y;
                    var inTileOffset = 0;

                    vertices[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector3(tileSize * x, tileSize * y);
                    uv[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector2(0, 0);
                    triangles[index * TRIANGLES_IN_QUAD + inTileOffset] = index * VERTECES_AND_UVS_IN_QUAD + inTileOffset;
                    inTileOffset++;
                    vertices[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector3(tileSize * x, tileSize * (y + 1));
                    uv[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector2(0, 1);
                    triangles[index * TRIANGLES_IN_QUAD + inTileOffset] = index * VERTECES_AND_UVS_IN_QUAD + inTileOffset;
                    inTileOffset++;
                    vertices[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector3(tileSize * (x + 1), tileSize * (y + 1));
                    uv[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector2(1, 1);
                    triangles[index * TRIANGLES_IN_QUAD + inTileOffset] = index * VERTECES_AND_UVS_IN_QUAD + inTileOffset;
                    inTileOffset++;
                    vertices[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector3(tileSize * (x + 1), tileSize * y);
                    uv[index * VERTECES_AND_UVS_IN_QUAD + inTileOffset] = new Vector2(1, 0);
                    triangles[index * TRIANGLES_IN_QUAD + inTileOffset] = index * VERTECES_AND_UVS_IN_QUAD + 0;
                    inTileOffset++;
                    triangles[index * TRIANGLES_IN_QUAD + inTileOffset] = index * VERTECES_AND_UVS_IN_QUAD + 2;
                    inTileOffset++;
                    triangles[index * TRIANGLES_IN_QUAD + inTileOffset] = index * VERTECES_AND_UVS_IN_QUAD + 3;
                } 
            }

            return new MeshProperties(vertices, uv, triangles);
        }
        
        public static MeshProperties CreateEmptyGridMeshProperties(int quadCount)
        {
            return new MeshProperties(new Vector3[VERTECES_AND_UVS_IN_QUAD * quadCount], new Vector2[VERTECES_AND_UVS_IN_QUAD * quadCount], new int[TRIANGLES_IN_QUAD * quadCount]);
        }
        
        public static void AddToMeshArrays(MeshProperties properties, int index, Vector3 pos, float rot, Vector3 baseSize, Vector2? uv00 = null, Vector2? uv11 = null) {
            //Relocate vertices
            var vIndex = index*4;
            var vIndex0 = vIndex;
            var vIndex1 = vIndex+1;
            var vIndex2 = vIndex+2;
            var vIndex3 = vIndex+3;
            
            uv00 ??= Vector2.zero;
            uv11 ??= Vector2.zero;

            baseSize *= .5f;

            var skewed = System.Math.Abs(baseSize.x - baseSize.y) > 0.1f;
            if (skewed) {
                properties.Vertices[vIndex0] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x,  baseSize.y);
                properties.Vertices[vIndex1] = pos+GetQuaternionEuler(rot)*new Vector3(-baseSize.x, -baseSize.y);
                properties.Vertices[vIndex2] = pos+GetQuaternionEuler(rot)*new Vector3( baseSize.x, -baseSize.y);
                properties.Vertices[vIndex3] = pos+GetQuaternionEuler(rot)*baseSize;
            } else {
                properties.Vertices[vIndex0] = pos+GetQuaternionEuler(rot-270)*baseSize;
                properties.Vertices[vIndex1] = pos+GetQuaternionEuler(rot-180)*baseSize;
                properties.Vertices[vIndex2] = pos+GetQuaternionEuler(rot- 90)*baseSize;
                properties.Vertices[vIndex3] = pos+GetQuaternionEuler(rot-  0)*baseSize;
            }
		
            //Relocate UVs
            var uv00V = (Vector2)uv00;
            var uv11V = (Vector2)uv11;
            properties.UV[vIndex0] = new Vector2(uv00V.x, uv11V.y);
            properties.UV[vIndex1] = new Vector2(uv00V.x, uv00V.y);
            properties.UV[vIndex2] = new Vector2(uv11V.x, uv00V.y);
            properties.UV[vIndex3] = new Vector2(uv11V.x, uv11V.y);
		
            //Create triangles
            var tIndex = index*6;
		
            properties.Triangles[tIndex+0] = vIndex0;
            properties.Triangles[tIndex+1] = vIndex3;
            properties.Triangles[tIndex+2] = vIndex1;
		
            properties.Triangles[tIndex+3] = vIndex1;
            properties.Triangles[tIndex+4] = vIndex3;
            properties.Triangles[tIndex+5] = vIndex2;
        }

        private static Quaternion GetQuaternionEuler(float angleOfRotation)
        {
            var rotation = Mathf.RoundToInt(angleOfRotation);
            rotation %= MAX_ANGLE_IN_DEGREESE;
            if (rotation < 0) rotation += MAX_ANGLE_IN_DEGREESE;
            if(_quaternionEulerCache == null) CacheQuaternionEuler();
            return _quaternionEulerCache[rotation];
        }

        private static void CacheQuaternionEuler()
        {
            if (_quaternionEulerCache != null) return;

            _quaternionEulerCache = new Quaternion[MAX_ANGLE_IN_DEGREESE];

            for (var i = 0; i < MAX_ANGLE_IN_DEGREESE; i++)
            {
                _quaternionEulerCache[i] = Quaternion.Euler(0, 0, i);
            }
        }
    }
}
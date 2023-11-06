using UnityEngine;

namespace Base.Math
{
    public record InSpaceCoordinate(float X = 0f, float Y = 0f, float Z = 0f)
    {
        public float X { get; } = X;
        public float Y { get; } = Y;
        public float Z { get; } = Z;

        public InSpaceCoordinate(Vector3 vector3) : this(vector3.x, vector3.y, vector3.z){}
        public InSpaceCoordinate(Vector3Int vector3) : this(vector3.x, vector3.y, vector3.z){}
        public InSpaceCoordinate(Vector2 vector2) : this(vector2.x, vector2.y){}
        public InSpaceCoordinate(Vector2Int vector2) : this(vector2.x, vector2.y){}
        
        public override string ToString()
        {
            return $"Coordinate is X: {X}, Y: {Y}, Z: {Z}."; }
    }
}
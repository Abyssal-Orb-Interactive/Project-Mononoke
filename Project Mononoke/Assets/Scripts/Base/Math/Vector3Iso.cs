using UnityEngine;

namespace Base.Math
{
    public struct Vector3Iso
    {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public Vector3Iso(Vector3 vector3)
        {
            var vector2Iso = new Vector2Iso(new Vector2(vector3.x, vector3.y));
            Z = vector3.z;
            X = vector2Iso.X;
            Y = vector2Iso.Y;
        }

        public Vector3Iso(float x, float y, float z)
        {
            Z = z;
            X = x;
            Y = y;
        }
        public Vector3 ToCartesian()
        {
            var z = Z;
            var vector2Iso = new Vector2Iso(this);
            var vector2 = vector2Iso.ToCartesian();
            return new Vector3(vector2.x, vector2.y, z);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
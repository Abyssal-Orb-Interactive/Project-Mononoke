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
        public static Vector3 ToCartesian(Vector3Iso vector3Iso)
        {
            float z = vector3Iso.Z;
            var vector2Iso = new Vector2Iso(vector3Iso);
            var vector2 = vector2Iso.ToCartesian();
            return new Vector3(vector2.x, vector2.y, z);
        }
    }
}
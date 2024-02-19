using UnityEngine;

namespace Base.Math
{
    public struct Vector2Iso
    {
        private const float X_ISOMETRIC_OFFSET = 0.5f;
        private const float Y_ISOMETRIC_OFFSET = 0.25f;
        private const float CARTESIAN_OFFSET = 2f;
        
        public readonly float X;
        public readonly float Y;
        
        public Vector2Iso(Vector2 vector2)
        {
            X = X_ISOMETRIC_OFFSET * (vector2.x + vector2.y);
            Y = Y_ISOMETRIC_OFFSET * (vector2.y - vector2.x);
        }
        
        public Vector2Iso(Vector3 vector3)
        {
            X = X_ISOMETRIC_OFFSET * (vector3.x + vector3.y);
            Y = Y_ISOMETRIC_OFFSET * (vector3.y - vector3.x);
        }

        public Vector2Iso(float x, float y)
        {
            X = x;
            Y = y;
        }
        
        public Vector2Iso(Vector3Iso vector3Iso)
        {
            X = vector3Iso.X;
            Y = vector3Iso.Y;
        }
        
        public Vector2 ToCartesian()
        {
            float cartX = X - CARTESIAN_OFFSET * Y;
            float cartY = X + CARTESIAN_OFFSET * Y;
            return new Vector2(cartX, cartY);
        }
    }
}
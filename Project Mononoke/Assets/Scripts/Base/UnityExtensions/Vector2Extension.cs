using UnityEngine;

namespace Base.UnityExtensions
{
    public static class Vector2Extension
    {
        private const float ISOMETRIC_OFFSET = 0.5f;
        
        public static Vector2 ToIsometric(this Vector2 vector)
        {
            return new Vector2(vector.x + vector.y, ISOMETRIC_OFFSET * (vector.y - vector.x));
        }
        
        public static Vector2 ToCartesian(this Vector2 vector)
        {
            return new Vector2(ISOMETRIC_OFFSET * vector.x - vector.y, ISOMETRIC_OFFSET * vector.y + vector.x);
        }
    }
}
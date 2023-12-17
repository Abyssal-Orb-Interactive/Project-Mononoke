using UnityEngine;

namespace Base.UnityExtensions
{
    public static class Vector3Extension
    {
        private const float ISOMETRIC_OFFSET = 0.5f;
        
        public static Vector3 ToIsometric(this Vector3 vector)
        {
            return new Vector3(vector.x + vector.y, ISOMETRIC_OFFSET * (vector.y - vector.x), 0);
        }
        
        public static Vector3 ToCartesian(this Vector3 vector)
        {
            return new Vector3(ISOMETRIC_OFFSET * vector.x - vector.y, ISOMETRIC_OFFSET * vector.y + vector.x, 0);
        }
    }
}
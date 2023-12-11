using UnityEngine;

namespace Base.Utils
{
    public static class VectorUtils
    {
        private const float ISOMETRIC_OFFSET = 0.5f;
        
        public static Vector3 ConvertFromCartesianToIsometric(Vector3 vector)
        {
            return new Vector3(vector.x + vector.y, ISOMETRIC_OFFSET * (vector.y - vector.x), 0);
        }
        
        public static Vector3 ConvertFromIsometricToCartesian(Vector3 vector)
        {
            return new Vector3(ISOMETRIC_OFFSET * vector.x - vector.y, ISOMETRIC_OFFSET * vector.y + vector.x, 0);
        }
    }
}
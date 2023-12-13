using UnityEngine;

namespace Base.Math
{
    public static class MotionProjector
    {
        public static Vector3 ProjectOnSurfaceNormal(Vector3 forward, Vector3 normal)
        {
            if (normal == Vector3.zero) return forward;
            return forward - Vector3.Dot(forward, normal) * normal;
        }
    }
}
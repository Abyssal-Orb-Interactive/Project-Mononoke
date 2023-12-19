using UnityEngine;

namespace Base.UnityExtensions
{
    /// <summary>
    /// Provides extension methods for converting vectors3 between isometric and Cartesian coordinate systems.
    /// </summary>
    public static class Vector3Extension
    {
        /// <summary>
        /// Converts a Vector3 from Cartesian to Isometric coordinates.
        /// </summary>
        /// <param name="cartesianVector3">The Vector3 in Cartesian coordinates.</param>
        /// <returns>The Vector3 in Isometric coordinates.</returns>
        public static Vector3 ToIsometric(this Vector3 cartesianVector3)
        {
            var z = cartesianVector3.z;
            var vector2 = (Vector2) cartesianVector3;
            var vector2Iso = vector2.ToIsometric();
            return new Vector3(vector2Iso.x, vector2Iso.y, z);
        }
        
        /// <summary>
        /// Converts a Vector3 from Isometric to Cartesian coordinates.
        /// </summary>
        /// <param name="isometricVector3">The Vector3 in Isometric coordinates.</param>
        /// <returns>The Vector3 in Cartesian coordinates.</returns>
        public static Vector3 ToCartesian(this Vector3 isometricVector3)
        {
            var z = isometricVector3.z;
            var vector2 = (Vector2) isometricVector3;
            var vector2Iso = vector2.ToCartesian();
            return new Vector3(vector2Iso.x, vector2Iso.y, z);
        }
    }
}
using UnityEngine;

namespace Base.UnityExtensions
{
    /// <summary>
    /// Provides extension methods for converting vectors2 between isometric and Cartesian coordinate systems.
    /// </summary>
    public static class Vector2Extension
    {
        private const float X_ISOMETRIC_OFFSET = 0.5f;
        private const float Y_ISOMETRIC_OFFSET = 0.25f;
        private const float CARTESIAN_OFFSET = 2f;
        
        /// <summary>
        /// Converts a Vector2 from Cartesian to Isometric coordinates.
        /// </summary>
        /// <param name="cartesianVector2">The Vector2 in Cartesian coordinates.</param>
        /// <returns>The Vector2 in Isometric coordinates.</returns>
        public static Vector2 ToIsometric(this Vector2 cartesianVector2)
        {
            var isoX = X_ISOMETRIC_OFFSET * (cartesianVector2.x + cartesianVector2.y);
            var isoY = Y_ISOMETRIC_OFFSET * (cartesianVector2.y - cartesianVector2.x);
            return new Vector2(isoX, isoY);
        }

        /// <summary>
        /// Converts a Vector2 from Isometric to Cartesian coordinates.
        /// </summary>
        /// <param name="isometricVector2">The Vector2 in Isometric coordinates.</param>
        /// <returns>The Vector2 in Cartesian coordinates.</returns>
        public static Vector2 ToCartesian(this Vector2 isometricVector2)
        {
            var cartX = isometricVector2.x - CARTESIAN_OFFSET * isometricVector2.y;
            var cartY = isometricVector2.x + CARTESIAN_OFFSET * isometricVector2.y;
            return new Vector2(cartX, cartY);
        }
    }
}
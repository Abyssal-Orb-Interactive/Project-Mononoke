using UnityEngine;

namespace Base.Math
{
    /// <summary>
    /// Represents a two-dimensional integer coordinate in a plane.
    /// </summary>
    public record InPlaneCoordinateInt(int X, int Y)
    {
        public int X { get; } = X;
        public int Y { get; } = Y;
        
        public InPlaneCoordinateInt(Vector3Int vector3) : this(vector3.x, vector3.y){}
        public InPlaneCoordinateInt(Vector3 vector3) : this(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.y)){}
        public InPlaneCoordinateInt(Vector2Int vector2) : this(vector2.x, vector2.y){}
        public InPlaneCoordinateInt(Vector2 vector2) : this(Mathf.FloorToInt(vector2.x),Mathf.FloorToInt(vector2.y)){}

        /// <summary>
        /// Returns a string representation of the coordinate.
        /// </summary>
        /// <returns>A string in the format "Coordinate is X: [X], Y: [Y]".</returns>
        public override string ToString()
        {
            return $"Coordinate is X: {X}, Y: {Y}.";
        }
    }
}
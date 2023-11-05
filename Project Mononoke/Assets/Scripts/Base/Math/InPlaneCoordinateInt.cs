namespace Base.Math
{
    /// <summary>
    /// Represents a two-dimensional integer coordinate in a plane.
    /// </summary>
    public record InPlaneCoordinateInt(int X, int Y)
    {
        public int X { get; } = X;
        public int Y { get; } = Y;

        /// <summary>
        /// Returns a string representation of the coordinate.
        /// </summary>
        /// <returns>A string in the format "Coordinate is X: [X], Y: [Y]".</returns>
        public override string ToString()
        {
            return $"Coordinate is X: {X}, Y: {Y}";
        }
    }
}
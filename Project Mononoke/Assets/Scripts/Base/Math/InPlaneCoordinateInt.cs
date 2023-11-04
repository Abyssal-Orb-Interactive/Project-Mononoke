namespace Base.Math
{
    public struct InPlaneCoordinateInt
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public InPlaneCoordinateInt(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
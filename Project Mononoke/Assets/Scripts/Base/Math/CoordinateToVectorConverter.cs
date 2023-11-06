using UnityEngine;

namespace Base.Math
{
    public static class CoordinateToVectorConverter
    {
        public static Vector3 ConvertInSpaceCoordinateToVector3(InSpaceCoordinate coordinate)
        {
            return new Vector3(coordinate.X, coordinate.Y, coordinate.Z);
        }
        
        public static Vector3 ConvertInPlaneCoordinateIntToVector3(InPlaneCoordinateInt coordinate)
        {
            return new Vector3(coordinate.X, coordinate.Y);
        }
    }
}
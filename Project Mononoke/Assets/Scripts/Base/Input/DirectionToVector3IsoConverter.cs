using Base.Input;
using Base.Math;
using UnityEngine;

namespace Source.Character.Movement
{
    public static class DirectionToVector3IsoConverter
    {
        public static Vector3 ToVector(MovementDirection direction)
        {
            var dir = DirectionToVector3Converter.ToVector(direction);
            var isoDir = new Vector3Iso(dir);
            return new Vector3(isoDir.X, isoDir.Y, isoDir.Z);
        }
    }
}
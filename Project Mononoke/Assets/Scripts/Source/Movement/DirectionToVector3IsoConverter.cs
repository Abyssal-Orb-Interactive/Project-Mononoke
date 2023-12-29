using Base.Input;
using Base.Math;
using UnityEngine;

namespace Source.Movement
{
    public static class DirectionToVector3Converter
    {
        private static readonly Vector3[] POSSIBLE_MOVEMENT_VECTORS =
        {
            new(-0.71f, 0.71f), new(0, 1), new(0.71f, 0.71f),
            new(-1, 0), new(0, 0), new(1, 0),
            new(-0.71f, -0.71f), new(0, -1), new(0.71f, -0.71f)
        };
        
        public static Vector3 ToVector(MovementDirection direction)
        {
            return direction switch
            {
                MovementDirection.Stay => POSSIBLE_MOVEMENT_VECTORS[4],
                MovementDirection.North => POSSIBLE_MOVEMENT_VECTORS[1],
                MovementDirection.NorthEast => POSSIBLE_MOVEMENT_VECTORS[2],
                MovementDirection.East => POSSIBLE_MOVEMENT_VECTORS[5],
                MovementDirection.SouthEast => POSSIBLE_MOVEMENT_VECTORS[8],
                MovementDirection.South => POSSIBLE_MOVEMENT_VECTORS[7],
                MovementDirection.SouthWest => POSSIBLE_MOVEMENT_VECTORS[6],
                MovementDirection.West => POSSIBLE_MOVEMENT_VECTORS[3],
                MovementDirection.NorthWest => POSSIBLE_MOVEMENT_VECTORS[0],
                _ => POSSIBLE_MOVEMENT_VECTORS[4].normalized
            };
        }
    }
}
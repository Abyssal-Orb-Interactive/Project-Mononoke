using Base.Input;
using UnityEngine;

namespace Source.Character.Movement
{
    public static class DirectionToVector3IsoConverter
    {
        private static readonly Vector3[] PossibleMovementVectors =
        {
            new(-0.71f, 0.71f), new(0, 1), new(0.71f, 0.71f),
            new(-1, 0), new(1, 0),
            new(-0.71f, -0.71f), new(0, -1), new(0.71f, -0.71f)
        };
        
        public static Vector3 ToVector(MovementDirection direction)
        {
            return direction switch
            {
                MovementDirection.North => PossibleMovementVectors[1],
                MovementDirection.NorthEast => PossibleMovementVectors[2],
                MovementDirection.East => PossibleMovementVectors[4],
                MovementDirection.SouthEast => PossibleMovementVectors[7],
                MovementDirection.South => PossibleMovementVectors[6],
                MovementDirection.SouthWest => PossibleMovementVectors[5],
                MovementDirection.West => PossibleMovementVectors[3],
                MovementDirection.NorthWest => PossibleMovementVectors[0],
                _ => Vector3.zero
            };
        }
    }
}
using UnityEngine;

namespace Base.Input
{
    public static class DirectionToVector3Converter
    {
        private static readonly Vector3[] PossibleMovementVectors =
        {
            new(-1, 1), new(0, 1), new(1, 1),
            new(-1, 0), new(1, 0),
            new(-1, -1), new(0, -1), new(1, -1)
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

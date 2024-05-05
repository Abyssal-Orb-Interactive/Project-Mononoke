using UnityEngine;

namespace Base.Input
{
    public static class DirectionToQuaternionConverter
    {
        private static readonly Quaternion[] _rotations = {
            Quaternion.Euler(0,0,0), Quaternion.Euler(0,0, -45f), Quaternion.Euler(0,0,-90f), 
            Quaternion.Euler(0,0, -135f),Quaternion.Euler(0,0, -180f), Quaternion.Euler(0,0, -225f),
            Quaternion.Euler(0,0, -270f), Quaternion.Euler(0,0, -315f) 
        };

        public static Quaternion GetQuaternionFor(MovementDirection direction)
        {
            return direction switch
            {
                MovementDirection.North => _rotations[0],
                MovementDirection.NorthEast => _rotations[1],
                MovementDirection.East => _rotations[2],
                MovementDirection.SouthEast => _rotations[3],
                MovementDirection.South => _rotations[4],
                MovementDirection.SouthWest => _rotations[5],
                MovementDirection.West => _rotations[6],
                MovementDirection.NorthWest => _rotations[7],
                _ => _rotations[0]
            };
        }
    }
}
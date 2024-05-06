using System;

namespace Base.Input
{
    public static class DirectionReverser
    {
        public static MovementDirection Reverse(MovementDirection direction)
        {
            return direction switch
            {
                MovementDirection.North => MovementDirection.South,
                MovementDirection.NorthEast => MovementDirection.SouthWest,
                MovementDirection.East => MovementDirection.West,
                MovementDirection.SouthEast => MovementDirection.NorthWest,
                MovementDirection.South => MovementDirection.North,
                MovementDirection.SouthWest => MovementDirection.NorthEast,
                MovementDirection.West => MovementDirection.East,
                MovementDirection.NorthWest => MovementDirection.SouthEast,
                _ => throw new Exception("No Implementation")
            };
        }
    }
}
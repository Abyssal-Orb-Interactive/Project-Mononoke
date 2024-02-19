using UnityEngine;
using Base.Input;

namespace Source.Character.Movement
{
    public struct PositionData
    {
        public MovementDirection Direction { get; private set; }
        public Vector3 Position { get; private set; }

        public PositionData(MovementDirection direction, Vector3 position)
        {
            Direction = direction;
            Position = position;
        }
    }
}


using Base.Grid;
using Base.Input;
using Base.Math;
using Source.BuildingModule;
using Source.Character.Movement;
using UnityEngine;

namespace Source.Character
{
    public class GridAnalyzer
    {
        private IsoCharacterMover _mover = null;
        private MovementDirection _facing = MovementDirection.East;
        private GroundGrid _grid = null;


        public GridAnalyzer(IsoCharacterMover mover, GroundGrid grid)
        {
            _mover = mover;
            _mover.MovementChanged += OnMovementChanged;
            _grid = grid;
        }

        private void OnMovementChanged(object sender, IsoCharacterMover.MovementActionEventArgs e)
        {
            _facing = e.Facing;
        }

        public bool TryFindBuildingNextToCharacter(out Building building)
        {
            
            var inGridPosition = GetInGridPosition();
            if (!_grid.HasBuildingAt(inGridPosition))
            {
                building = null;
                return false;
            }
            building = _grid.GetBuildingAt(inGridPosition);
            return true;
        }

        private Vector3Int GetInGridPosition()
        {
            var offset = DirectionToVector3IsoConverter.ToVector(_facing);
            var position = _mover.GetPositionData().Position;
            var targetPosition = position + offset;
            var inGridPosition = _grid.WorldToGrid(targetPosition);
            return inGridPosition;
        }
    }
}
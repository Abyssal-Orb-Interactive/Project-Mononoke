using System;
using Source.BuildingModule;
using UnityEngine;
using VContainer;

namespace Source.Character
{
    public class CharacterLogicIsometric2DCollider : MonoBehaviour
    {
        private GridAnalyzer _gridAnalyzer = null;
        private Building _currentBuilding = null;
        public event Action<Building> BuildingInCollider, BuildingOutCollider;
        [Inject] public void Initialize(GridAnalyzer gridAnalyzer)
        {
            _gridAnalyzer = gridAnalyzer;
        }

        public void FrameByFrameCalculate()
        {
            if (_gridAnalyzer.TryFindBuildingNextToCharacter(out var building))
            {
                if(_currentBuilding == building) return;
                _currentBuilding = building;
                BuildingInCollider?.Invoke(_currentBuilding);
            }
            else if(_currentBuilding != null)
            {
                BuildingOutCollider?.Invoke(_currentBuilding);
                _currentBuilding = null;
            }
        }
    }
}
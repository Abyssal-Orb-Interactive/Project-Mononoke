using System;
using Source.BuildingModule;
using UnityEngine;

namespace Source.Character
{
    public class CharacterLogicIsometric2DCollider : MonoBehaviour
    {
        private GridAnalyzer _gridAnalyzer = null;
        public event Action<Building> BuildingInCollider; 

        public void Initialize(GridAnalyzer gridAnalyzer)
        {
            _gridAnalyzer = gridAnalyzer;
        }

        public void FrameByFrameCalculate()
        {
            if (_gridAnalyzer.TryFindBuildingNextToCharacter(out var building))
            {
                Debug.Log("Building here");
                BuildingInCollider?.Invoke(building);
            }
        }
    }
}
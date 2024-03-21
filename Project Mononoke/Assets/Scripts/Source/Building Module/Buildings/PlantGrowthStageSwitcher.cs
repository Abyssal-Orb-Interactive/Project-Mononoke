using System;
using Source.BuildingModule.Buildings.Visual;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    public class PlantGrowthStageSwitcher
    {
        public int CurrentGrowthStageIndex { get; private set; } = 0;
        public event Action<int> GrowthStageChanged = null;
        private int _presentsOfOneStage = 0;

        public int NumberOfStages { get; private set; } = 0;

        public PlantGrowthStageSwitcher(Plant plant,int numberOfStages)
        {
            NumberOfStages = numberOfStages;
            CurrentGrowthStageIndex = 1;
            CalculatePresentsOfOneStage();
            plant.PlantGrown += SwitchStageIfNeeded;
            plant.PlantMatured += SwitchToLastStage;
            plant.PlantStartedGrow += () =>  GrowthStageChanged?.Invoke(CurrentGrowthStageIndex);
        }

        private void SwitchToLastStage()
        {
            CurrentGrowthStageIndex = NumberOfStages;
            GrowthStageChanged?.Invoke(CurrentGrowthStageIndex);
        }

        private void CalculatePresentsOfOneStage()
        {
            _presentsOfOneStage = 100 / (NumberOfStages - 1);
        }

        private void TryGoToNextStage()
        {
            if (CurrentGrowthStageIndex >= NumberOfStages) return;
            CurrentGrowthStageIndex++;
            GrowthStageChanged?.Invoke(CurrentGrowthStageIndex);
        }

        private void SwitchStageIfNeeded(float elapsedTimeInPresents)
        {
            if (elapsedTimeInPresents >= CurrentGrowthStageIndex * _presentsOfOneStage)
            {
                TryGoToNextStage();
            }
        }
    }
}
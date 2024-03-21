using System;
using Base.Timers;
using Source.BuildingModule.Buildings.Visual;
using Source.ItemsModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    public class Plant
    {
        private Timer _plantGrownTimer = null;
        private PlantGrowthStageSwitcher _stageSwitcher = null;
        public event Action<float> PlantGrown;
        public event Action PlantMatured;
        public event Action PlantStartedGrow;

        public Plant(SeedData seedData, PlantSprite sprite)
        {
            _plantGrownTimer = TimersFabric.Create(Timer.TimerType.ScaledSecond, seedData.MaxGrownTimeInSeconds);
            _stageSwitcher = new PlantGrowthStageSwitcher(this, seedData.PlantGrowthStagesSprites.Count);
            
            _plantGrownTimer.TimerTicked += OnPlantGrown;
            _plantGrownTimer.TimerFinished += () =>
            {
                Debug.Log($"{seedData.Name} grown");
                _plantGrownTimer.TimerTicked -= OnPlantGrown;
                PlantMatured?.Invoke();
            };
            
            sprite.Initialize(_stageSwitcher, seedData.PlantGrowthStagesSprites);
            PlantStartedGrow?.Invoke();
            _plantGrownTimer.Start();
            
        }

        private void OnPlantGrown()
        {
            PlantGrown?.Invoke(_plantGrownTimer.ElapsedTimeInPresents);
        }
    }
}
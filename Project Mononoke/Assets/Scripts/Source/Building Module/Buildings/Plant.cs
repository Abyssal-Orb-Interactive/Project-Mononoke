using System;
using Base.Timers;
using Source.BuildingModule.Buildings.Visual;
using Source.ItemsModule;

namespace Source.BuildingModule.Buildings
{
    public class Plant
    {
        private readonly Timer _plantGrownTimer = null;
        private PlantGrowthStageSwitcher _stageSwitcher = null;
        public event Action<float> PlantGrown;
        public event Action PlantMatured, PlantStartedGrow;

        public Plant(SeedData seedData, PlantSprite sprite)
        {
            _plantGrownTimer = TimersFabric.Create(Timer.TimerType.ScaledSecond, seedData.GrownTimeInSeconds);
            _stageSwitcher = new PlantGrowthStageSwitcher(this, seedData.PlantGrowthStagesSprites.Count);
            _plantGrownTimer.TimerTicked += OnPlantGrown;
            _plantGrownTimer.TimerFinished += () =>
            {
                _plantGrownTimer.TimerTicked -= OnPlantGrown;
                PlantMatured?.Invoke();
            };
            
            sprite.Initialize(_stageSwitcher, seedData.PlantGrowthStagesSprites);
        }

        public void StartGrow()
        {
            PlantStartedGrow?.Invoke();
            _plantGrownTimer.Start();
        }

        private void OnPlantGrown()
        {
            PlantGrown?.Invoke(_plantGrownTimer.ElapsedTimeInPresents);
        }
    }
}
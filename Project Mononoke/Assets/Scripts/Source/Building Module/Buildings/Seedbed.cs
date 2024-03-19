using System;
using Base.Timers;
using Source.BuildingModule.Buildings.Visual;
using Source.ItemsModule;
using UnityEngine;
using VContainer;

namespace Source.BuildingModule.Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Seedbed : MonoBehaviour
    {
        private Item<SeedData> _seed = null;
        private Timer _seedGrownTimer = null;
        private SpriteRenderer _renderer = null;

        public event Action<SpriteRenderer> GrowthStageChanged = null;

        private void OnValidate()
        {
            _renderer ??= GetComponent<SpriteRenderer>();
        }

        public void Plant(Item<SeedData> seed)
        {
            _seed = seed;
            _seed.Database.TryGetItemDataBy(_seed.ID, out var seedData);

            _seedGrownTimer = TimersFabric.Create(Timer.TimerType.ScaledSecond, seedData.MaxGrownTimeInSeconds);
            _seedGrownTimer.TimerFinished += () => Debug.Log($"{seedData.Name} grown");
            _seedGrownTimer.Start();
        }

        protected void OnGrowthStageChanged()
        {
            GrowthStageChanged?.Invoke(_renderer);
        }
    }
}
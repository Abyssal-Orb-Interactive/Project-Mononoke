using Base.Timers;
using Source.ItemsModule;
using UnityEngine;
using VContainer;

namespace Source.BuildingModule.Buildings
{
    public class Seedbed : MonoBehaviour
    {
        private Item<SeedData> _seed = null;
        private Timer _seedGrownTimer = null;

       [Inject] public void Initialize(Item<SeedData> seed)
        {
            _seed = seed;
            _seed.Database.TryGetItemDataBy(_seed.ID, out var seedData);

            _seedGrownTimer = TimersFabric.Create(Timer.TimerType.ScaledSecond, seedData.MaxGrownTimeInSeconds);
            _seedGrownTimer.TimerFinished += () => Debug.Log($"{seedData.Name} grown");
            _seedGrownTimer.Start();
        }
    }
}
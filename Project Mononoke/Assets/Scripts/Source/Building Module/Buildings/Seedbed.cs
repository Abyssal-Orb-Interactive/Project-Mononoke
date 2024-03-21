using System;
using Source.BuildingModule.Buildings.Visual;
using Source.ItemsModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Seedbed : MonoBehaviour
    {
        private Plant _plant = null;
        [SerializeField] private PlantSprite _plantSprite = null;

        private void OnValidate()
        {
            _plantSprite ??= GetComponentInChildren<PlantSprite>();
        }

        public void Plant(Item<SeedData> seed)
        {
            seed.Database.TryGetItemDataBy(seed.ID, out var seedData);
            _plant = new Plant(seedData, _plantSprite);
        }
    }
}
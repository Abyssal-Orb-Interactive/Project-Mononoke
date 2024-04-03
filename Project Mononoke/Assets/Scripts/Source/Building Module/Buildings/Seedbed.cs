using System;
using Source.BuildingModule.Buildings.Visual;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Seedbed : Building
    {
        private Plant _plant = null;
        [SerializeField] private PlantSprite _plantSprite = null;

        private void OnValidate()
        {
            _plantSprite ??= GetComponentInChildren<PlantSprite>();
        }

        public void Plant(Item seed)
        {
            seed.Database.TryGetItemDataBy(seed.ID, out var seedData);
            if(seedData is not SeedData data) return;
            _plant = new Plant(data, _plantSprite);
        }
        
        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            var holdingItem = pickUpper.Manipulator.Item;
            Plant(holdingItem);
        }
        

        private void Plant(SeedData seedData)
        {
            _plant = new Plant(seedData, _plantSprite);
        }
    }
}
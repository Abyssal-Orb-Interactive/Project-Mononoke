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
            var seedData = seed.Data;
            if(seedData is not SeedData data) return;
            _plant = new Plant(data, _plantSprite);
            _plant.PlantStartedGrow += OnPlantStartedGrow;
            _plant.PlantMatured += OnPlantMatured;
            _plantSprite.Initialize(new PlantGrowthStageSwitcher(_plant, data.PlantGrowthStagesSprites.Count - 1), data.PlantGrowthStagesSprites);
            _plant.StartGrow();
        }

        private void OnPlantMatured()
        {
            ReadyToInteract = true;
        }

        private void OnPlantStartedGrow()
        {
            ReadyToInteract = false;
        }

        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            var holdingItem = pickUpper.Manipulator.Item;
            Plant(holdingItem);
        }
    }
}
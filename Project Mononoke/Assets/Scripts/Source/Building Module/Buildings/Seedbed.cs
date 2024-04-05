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
        public enum Stages
        {
            Empty,
            Growing,
            Grown
        }
        
        
        private Plant _plant = null;
        private Stages _stage = Stages.Empty;
        private SeedData _seedData = null;
        [SerializeField] private PlantSprite _plantSprite = null;

        private void OnValidate()
        {
            _plantSprite ??= GetComponentInChildren<PlantSprite>();
        }

        public void Plant(Item seed)
        {
            var seedData = seed.Data; 
            if(seedData is not SeedData data) return;
            _seedData = data;
            _plant = new Plant(_seedData, _plantSprite);
            _plant.PlantStartedGrow += OnPlantStartedGrow;
            _plant.PlantMatured += OnPlantMatured;
            _plant.StartGrow();
            _stage = Stages.Growing;
        }

        private void OnPlantMatured()
        {
            ReadyToInteract = true;
            _stage = Stages.Grown;
        }

        private void OnPlantStartedGrow()
        {
            ReadyToInteract = false;
        }

        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            var holdingItem = pickUpper.Manipulator.Item;
            switch (_stage)
            {
                case Stages.Empty:
                    Plant(holdingItem);
                    break;
                case Stages.Growing:
                    return;
                case Stages.Grown:
                    if (!_seedData.FruitDatabase.TryGetItemDataBy(_seedData.FruitDatabaseID, out var fruit)) return;
                    _plant.ClearVisual();
                    for (var i = 0; i < _seedData.GrownFruits; i++)
                    {
                        ItemViewFabric.Create(new Item(fruit), new Vector3(0.5f, 1));
                    }
                    _stage = Stages.Empty;
                    _seedData = null;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
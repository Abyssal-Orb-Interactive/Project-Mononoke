using System;
using Source.BuildingModule.Buildings.Visual;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using VContainer;

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
        private InventoryPresenter _inventoryPresenter = null;

        private void OnValidate()
        {
            _plantSprite ??= GetComponentInChildren<PlantSprite>();
        }

        [Inject]
        public void Initialize(InventoryPresenter inventoryPresenter)
        {
            _inventoryPresenter = inventoryPresenter;
            _inventoryPresenter.ItemChosen += OnItemChosen;
        }

        private void OnItemChosen(Item item)
        {
            TryPlant(item);
        }

        private bool TryPlant(Item seed)
        {
            var seedData = seed?.Data; 
            if(seedData is not SeedData data) return false;
            Plant(data);
            return true;
        }
        
        public void Plant(SeedData data)
        {
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
                    if (!TryPlant(holdingItem))
                    {
                        _inventoryPresenter.UpdateChooseMenuWith(InventoryFilters.Seeds);
                        _inventoryPresenter.GetOnItemChoosingMenu();
                        ReadyToInteract = false;
                    }
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

                    for (var i = 0; i < _seedData.GrownSeeds; i++)
                    {
                        ItemViewFabric.Create(new Item(_seedData), new Vector3(0.5f, 1));
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
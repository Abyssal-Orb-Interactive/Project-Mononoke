using System;
using Base.Input;
using Base.Math;
using Source.BuildingModule.Buildings.Visual;
using Source.Character.Movement;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Source.BuildingModule.Buildings
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(ItemLauncher))]
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
        private ItemLauncher _launcher = null;

        private void OnValidate()
        {
            _plantSprite ??= GetComponentInChildren<PlantSprite>();
        }

        [Inject]
        public void Initialize(InventoryPresenter inventoryPresenter)
        {
            _inventoryPresenter = inventoryPresenter;
            _inventoryPresenter.ItemChosen += OnItemChosen;
            _launcher = GetComponent<ItemLauncher>();
        }

        private void OnItemChosen(Item item, Building building)
        {
            if(building != this) return;
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
                        _inventoryPresenter.GetOnItemChoosingMenu(this);
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
                        _launcher.DropAndGetEndingDropPosition(new Item(fruit));
                    }
                    for (var i = 0; i < _seedData.GrownSeeds; i++)
                    {
                        _launcher.DropAndGetEndingDropPosition(new Item(_seedData));
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
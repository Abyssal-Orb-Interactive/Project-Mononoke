using System.Collections.Generic;
using Base.DIContainer;
using Base.GameLoop;
using Base.Grid;
using Scripts.Source.Craft_Module;
using Source.BattleSystem.UI;
using Source.Formations;
using Source.InventoryModule;
using Source.ItemsModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    public class PresetBuildingsInGridInitializer : MonoBehaviour
    {
        private GroundGrid _grid = null;
        private readonly List<Building> _initializedBuildings = new();
        [SerializeField] private Receipt _receipt = null;
        [SerializeField] private TestArmy _playerArmy = null;
        [SerializeField] private OnGridObjectPlacer _placer = null;
        [SerializeField] private GameLifetimeScope _scope = null;
        [SerializeField] private GameLoop _gameLoop = null;
        [SerializeField] private Transform _minionsHolder = null;
        [SerializeField] private HealthBarsCanvas _healthBarsCanvas = null;

        public void Initilize(GroundGrid grid)
        {
            _grid = grid;
            TryAddAllPresetsToGrid();
        }

        private void TryAddAllPresetsToGrid()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.TryGetComponent<Building>(out var building))
                {
                    Destroy(child.gameObject);
                    continue;
                }

                if (TryAddBuildingToGrid(building))
                {}
            }
        }

        private bool TryAddBuildingToGrid(Building building)
        {
            var inGridPosition = _grid.WorldToGrid(building.transform.position);
            if (_grid.HasBuildingAt(inGridPosition) || !_grid.IsCellPassableAt(inGridPosition))
            {
                Destroy(building.gameObject);
                return false;
            }

            if (_grid.TryAddBuilding(building, inGridPosition))
            {
                _initializedBuildings.Add(building);
                var craftWorkshop = building as CraftWorkshop;
                if (craftWorkshop != null)
                {
                    var receipts = new List<Receipt> { _receipt };
                    craftWorkshop.Initialize(new Inventory(), receipts, _playerArmy, _scope, _placer, _gameLoop, _minionsHolder, _healthBarsCanvas);
                }

                if (building.TryGetComponent<ItemLauncher>(out var thrower))
                {
                    thrower.Initialize(_grid);
                }
                
                return true;
            }
            
            Destroy(building.gameObject);
            return false;
        }
    }
}
using System;
using System.Linq;
using Base.Grid;
using Source.BuildingModule.Buildings;
using Source.InventoryModule;
using UnityEngine;
using static Source.BuildingModule.IBuildRequester;

namespace Source.BuildingModule
{
    public class OnGridBuilder : IDisposable
    {
        private readonly OnGridObjectPlacer _objectPlacer = null;
        private readonly GroundGrid _grid = null;
        private readonly BuildingsDatabaseSo _templatesDatabase = null;
        private readonly ObjectContainersAssociator _containerAssociator = null;
        private readonly InventoryPresenter _inventoryPresenter = null;

        public OnGridBuilder(OnGridObjectPlacer objectPlacer, GroundGrid grid, BuildingsDatabaseSo buildingsDatabase, ObjectContainersAssociator containersAssociator, InventoryPresenter inventoryPresenter)
        {
            _objectPlacer = objectPlacer;
            _grid = grid;
            _templatesDatabase = buildingsDatabase;
            _containerAssociator = containersAssociator;
            _inventoryPresenter = inventoryPresenter;
            BuildingRequestsBus.Subscribe(HandleBuildRequest);
        }

        public void Dispose()
        {
            BuildingRequestsBus.Unsubscribe(HandleBuildRequest);
        }

        public bool TryBuildBuildingWith(int ID, Vector3 position)
        {
            var gridPosition = _grid.WorldToGrid(position);

            if (_grid.HasBuildingAt(gridPosition))
            {
                Debug.LogWarning($"Building already present at the specified position {gridPosition}.");
                return false;
            }

            var buildingData = _templatesDatabase.BuildingsData.FirstOrDefault(data => data.ID == ID);

            if (buildingData == null)
            {
                Debug.LogError($"Building with ID {ID} not found in the database.");
                return false;
            }

            var buildingPrefab = buildingData.Prefab;
            Transform container = _containerAssociator.Associations[ID];

            var objectPlacementInformation = new ObjectPlacementInformation<Building>(buildingPrefab, gridPosition, Quaternion.identity, container);

            var building = _objectPlacer.PlaceObject(objectPlacementInformation);
            var seedBed = building as Seedbed;
            seedBed.Initialize(_inventoryPresenter);

            return _grid.TryAddBuilding(building, gridPosition);
        }

        private void HandleBuildRequest(BuildRequestEventArgs args)
        {
            TryBuildBuildingWith(args.BuildingID, args.Position);
        }
    }
}

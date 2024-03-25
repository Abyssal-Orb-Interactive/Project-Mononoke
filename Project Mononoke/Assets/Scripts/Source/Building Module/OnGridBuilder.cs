using System;
using System.Linq;
using Base.Grid;
using Source.BuildingModule.Buildings;
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

        public OnGridBuilder(OnGridObjectPlacer objectPlacer, GroundGrid grid, BuildingsDatabaseSo buildingsDatabase, ObjectContainersAssociator containersAssociator)
        {
            _objectPlacer = objectPlacer;
            _grid = grid;
            _templatesDatabase = buildingsDatabase;
            _containerAssociator = containersAssociator;
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

            BuildingData buildingData = _templatesDatabase.BuildingsData.FirstOrDefault(data => data.ID == ID);

            if (buildingData == null)
            {
                Debug.LogError($"Building with ID {ID} not found in the database.");
                return false;
            }

            Seedbed buildingPrefab = buildingData.Prefab;
            Transform container = _containerAssociator.Associations[ID];

            var objectPlacementInformation = new ObjectPlacementInformation<Seedbed>(buildingPrefab, position, Quaternion.identity, container);

            var building = _objectPlacer.PlaceObject(objectPlacementInformation);
        
            _grid.TryAddBuilding(building, gridPosition);
            
            return true;
        }

        private void HandleBuildRequest(BuildRequestEventArgs args)
        {
            TryBuildBuildingWith(args.BuildingID, args.Position);
        }
    }
}

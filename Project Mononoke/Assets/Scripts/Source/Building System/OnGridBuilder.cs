using System.Linq;
using Base.Grid;
using UnityEngine;

namespace Source.BuildingSystem
{
    public class OnGridBuilder
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
        }

        public bool TryBuildBuildingWith(int ID, Vector3 position)
        {
            Vector3Int gridPosition = _grid.WorldToGrid(position);

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

            GameObject buildingPrefab = buildingData.Prefab;
            Transform container = _containerAssociator.Associations[ID];

            var objectPlacementInformation = new ObjectPlacementInformation(buildingPrefab, position, Quaternion.identity, container);

            var building = _objectPlacer.PlaceObject(objectPlacementInformation);
        
            _grid.TryAddBuilding(building, gridPosition);

            return true;
        }
    }
}

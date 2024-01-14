using System.Linq;
using Base.Grid;
using UnityEngine;

namespace Source.BuildingSystem
{
    public class OnGridBuilder
    {
        private OnGridObjectPlacer _objectPlacer = null;
        private GroundGrid _grid = null;
        private BuildingsDatabaseSo _templatesDatabase = null;

        public OnGridBuilder(OnGridObjectPlacer objectPlacer, GroundGrid grid, BuildingsDatabaseSo buildingsDatabase)
        {
            _objectPlacer = objectPlacer;
            _grid = grid;
            _templatesDatabase = buildingsDatabase;
        }

        public void BuildBuildingWith(int ID, Vector3 position)
        {
            GameObject buildingTemplate = _templatesDatabase.BuildingsData.First(data => data.ID == ID).Prefab;
            var objectPlacementInformation = new ObjectPlacementInformation(buildingTemplate, position, Quaternion.identity, null);
        }
    }
}

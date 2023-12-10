using System.Linq;
using Base.Utils;
using Source.Input;
using UnityEngine;

namespace Source.Building_System
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        [SerializeField] private Grid _grid;
        [SerializeField] private BuildingsDatabaseSo _database;

        private int _selectedBuildingID = -1;


        public void StartPlacement(int id)
        {
            _selectedBuildingID = _database.BuildingsData.First(data => data.ID == id).ID;
            if (_selectedBuildingID == -1)
            {
                Debug.LogError($"No {id} found in buildings database {_database}");
                return;
            }
            _inputManager.AddClickAction(PlaceStructure);
            _inputManager.AddExitAction(StopPlacement);
        }

        private void StopPlacement()
        {
            _selectedBuildingID = -1;
            _inputManager.RemoveClickAction(PlaceStructure);
            _inputManager.RemoveExitAction(StopPlacement);
        }

        private void PlaceStructure()
        {
            var gridPosition = _grid.WorldToCell(MouseUtils.GetMouseWorldPosWithoutZUsingNewInputSystem());
            var building = Instantiate(_database.BuildingsData[_selectedBuildingID].Prefab);
            building.transform.position = _grid.CellToWorld(gridPosition);
        }
    }
}

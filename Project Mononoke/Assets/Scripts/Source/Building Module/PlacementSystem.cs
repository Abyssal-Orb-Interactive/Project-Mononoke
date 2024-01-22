using System.Linq;
using Base.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;
using InputHandler = Base.Input.InputHandler;

namespace Source.BuildingModule
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private BuildingsDatabaseSo _database;

        private int _selectedBuildingID = -1;
        private InputHandler _inputHandler = null;
        private Mouse _cursor = null;

        private void OnValidate()
        {
            _cursor ??= Mouse.current;
        }

        public void StartPlacement(int id)
        {
            _selectedBuildingID = _database.BuildingsData.First(data => data.ID == id).ID;
            if (_selectedBuildingID == -1)
            {
                Debug.LogError($"No {id} found in buildings database {_database}");
                return;
            }
            _inputHandler.AddInputChangedHandler(PlaceStructure);
            _inputHandler.AddInputChangedHandler(StopPlacement);
        }

        private void StopPlacement(object sender, InputHandler.InputActionEventArgs e)
        {
            if(e.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            _selectedBuildingID = -1;
            _inputHandler.RemoveInputChangedHandler(PlaceStructure);
            _inputHandler.RemoveInputChangedHandler(StopPlacement);
        }

        private void PlaceStructure(object sender, InputHandler.InputActionEventArgs e)
        {
            if(e.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            
            Vector3Int gridPosition = _grid.WorldToCell(_cursor.GetMouseWorldPosition());
            GameObject building = Instantiate(_database.BuildingsData[_selectedBuildingID].Prefab);
            building.transform.position = _grid.CellToWorld(gridPosition);
        }
    }
}

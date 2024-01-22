using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Source.BuildingModule
{
    public class ObjectContainersAssociator : MonoBehaviour
    {
        [SerializeField] private Transform _containersHolder = null;
        [SerializeField] private BuildingsDatabaseSo _buildingsDatabase = null;

        private Dictionary<int, Transform> _associations = null;
        public IReadOnlyDictionary<int, Transform> Associations 
        {
            get
            {
                return _associations ??= GetRegisteredAssociations();
            }
        }

        private Dictionary<int, Transform> GetRegisteredAssociations()
        {
            var associations = new Dictionary<int, Transform>();
            var containersDictionary = _containersHolder
                .Cast<Transform>()
                .ToDictionary(container => container.GetComponent<BuildingsContainer>().RequestedBuildingId);

            if (_containersHolder.childCount - 1 < _buildingsDatabase.BuildingsData.Count)
            {
                Debug.LogWarning($"Containers count {_containersHolder.childCount - 1} doesn't match with number of buildings variants {_buildingsDatabase.BuildingsData.Count}, all unassociated building will be moved into default container: {_containersHolder.GetChild(0)}");
            }

            foreach (var building in _buildingsDatabase.BuildingsData)
            {
                if (containersDictionary.TryGetValue(building.ID, out var container))
                {
                    associations.Add(building.ID, container);
                }
                else
                {
                    associations.Add(building.ID, _containersHolder.GetChild(0));
                }
            }

            return associations;
        }
    }
}


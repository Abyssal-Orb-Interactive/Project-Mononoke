using UnityEngine;

namespace Source.BuildingModule
{
    public class BuildingsContainer : MonoBehaviour
    {
        [SerializeField] private int _requestedBuildingId = -1;

        public int RequestedBuildingId => _requestedBuildingId;
    }
}


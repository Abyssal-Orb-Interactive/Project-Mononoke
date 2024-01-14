using System;
using Base.Grid;
using UnityEngine;
using VContainer;

namespace Source.BuildingSystem
{
    
    
    public class OnGridObjectPlacer : MonoBehaviour 
    {
        [SerializeField] private float _gridYOffset = -0.25f;

        public void PlaceObject(ObjectPlacementInformation placementData)
        {
            var correctedPosition = CorrectPosition(placementData);
            GameObject placedObject = Instantiate(placementData.Prefab, correctedPosition, placementData.Rotation, placementData.Parent);
        }

        private Vector3 CorrectPosition(ObjectPlacementInformation placementData)
        {
            return new Vector3(placementData.Position.x, placementData.Position.y + _gridYOffset, placementData.Position.z);
        }

    }
}

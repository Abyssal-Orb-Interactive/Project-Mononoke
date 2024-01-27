using UnityEngine;

namespace Source.BuildingModule
{


    public class OnGridObjectPlacer : MonoBehaviour 
    {
        [SerializeField] private float _gridYOffset = -0.25f;

        public T PlaceObject<T>(ObjectPlacementInformation<T> placementData) where T : Object
        {
            var correctedPosition = CorrectPosition(placementData);
            T placedObject = Instantiate(placementData.Prefab, correctedPosition, placementData.Rotation, placementData.Parent);

            return placedObject;
        }

        private Vector3 CorrectPosition<T>(ObjectPlacementInformation<T> placementData) where T : Object
        {
            return new Vector3(placementData.Position.x, placementData.Position.y + _gridYOffset, placementData.Position.z);
        }
    }
}

using Base.Math;
using UnityEngine;

namespace Source.BuildingModule
{


    public class OnGridObjectPlacer : MonoBehaviour 
    {
        public T PlaceObject<T>(ObjectPlacementInformation<T> placementData) where T : Object
        {
            var correctedPosition = CorrectPosition(placementData);
            var placedObject = Instantiate(placementData.Prefab, correctedPosition, placementData.Rotation, placementData.Parent);

            return placedObject;
        }

        private Vector3 CorrectPosition<T>(ObjectPlacementInformation<T> placementData) where T : Object
        {
            var isoPosition = new Vector3Iso(placementData.Position);
            return new Vector3(isoPosition.X, isoPosition.Y, isoPosition.Z);
        }
    }
}

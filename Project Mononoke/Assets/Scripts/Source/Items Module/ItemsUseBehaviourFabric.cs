using Base.Input;
using Source.BuildingModule;
using Source.Character.Movement;
using UnityEngine;

namespace Source.ItemsModule
{
    public static class ItemsUseBehaviourFabric
    {
        public static UseBehaviour GetBehaviour(string itemID)
        {
            return itemID switch
            {
                "Hoe" => MakeBuildRequestMatterIn,
                _ => _ => Debug.Log("Here2")
            };
        }
        
        private static void MakeBuildRequestMatterIn(object context)
        {
            if(context is not IPositionSource positionSource) return;
            var position = positionSource.GetPosition();
            var targetPosition = CalculateTargetPositionUsing(position);
            Debug.Log(targetPosition);
            BuildingRequestsBus.MakeRequest(new IBuildRequester.BuildRequestEventArgs(0, targetPosition));
        }
        
        private static Vector3 CalculateTargetPositionUsing(PositionData data)
        {
            var position = data.Position;//IPositionSource must return Cartesian pos
            Debug.Log(position);
            var facing = data.Direction;
            Debug.Log(facing);
            var oneFacingVector = DirectionToVector3Converter.ToVector(facing);
            Debug.Log(oneFacingVector);
            var roundedPosition = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
            Debug.Log(roundedPosition);
            return roundedPosition + oneFacingVector;
        }
    }
}
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
            BuildingRequestsBus.MakeRequest(new IBuildRequester.BuildRequestEventArgs(0, targetPosition));
        }
        
        private static Vector3 CalculateTargetPositionUsing(PositionData data)
        {
            var position = data.Position;//IPositionSource must return Cartesian pos
            var facing = data.Direction;
            var oneFacingVector = DirectionToVector3Converter.ToVector(facing);
            var roundedPosition = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
            return roundedPosition + oneFacingVector;
        }
    }
}
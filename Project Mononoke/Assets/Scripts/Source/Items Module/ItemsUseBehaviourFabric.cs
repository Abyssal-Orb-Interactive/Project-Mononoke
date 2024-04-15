using Base.Input;
using Base.Math;
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
            var position = positionSource.GetPositionData();
            var targetPosition = CalculateTargetPositionUsing(position);
            BuildingRequestsBus.MakeRequest(new IBuildRequester.BuildRequestEventArgs(0, targetPosition));
        }
        
        private static Vector3 CalculateTargetPositionUsing(PositionData data)
        {
            var position = data.Position;//IPositionSource must return Iso pos
            var facing = data.Direction;
            var oneFacingVector = DirectionToVector3Converter.ToVector(facing);
            var oneFacingVectorIso = new Vector2Iso(oneFacingVector);
            return new Vector3(position.x + oneFacingVectorIso.X, position.y + oneFacingVectorIso.Y);
        }
    }
}
using Base.Input;
using Source.BuildingModule;
using UnityEngine;
using Source.Character.Movement;

namespace Source.ItemsModule
{
    public class BuildingTool : Item<ItemData>, IBuildRequester
    {
        public delegate void UseAction(object context);
        public UseAction OnUse;
        private Vector3 _targetBuildingPosition = Vector3.zero;


        public BuildingTool(int iD, ItemsDatabase<ItemData> database, int buildingID) : base(iD, database, buildingID)
        {
            OnUse = _ => MakeRequest(new IBuildRequester.BuildRequestEventArgs(buildingID, _targetBuildingPosition));
        }

        public void MakeRequest(IBuildRequester.BuildRequestEventArgs args)
        {
            BuildingRequestsBus.MakeRequest(this, args);
        }

        public void UseMatterIn(object context)
        {
            if(context is not IPositionSource positionSource) return;
            
            var position = positionSource.GetPosition();
            _targetBuildingPosition = CalculateTargetPositionUsing(position);
            Debug.Log(_targetBuildingPosition);

            OnUse?.Invoke(positionSource);
        }

        private Vector3 CalculateTargetPositionUsing(PositionData data)
        {
            var position = data.Position; //IPositionSource must return Cartesian pos
            var facing = data.Direction;
            var oneFacingVector = DirectionToVector3Converter.ToVector(facing);
            var roundedPosition = new Vector3(Mathf.Ceil(position.x), Mathf.Ceil(position.y), Mathf.Ceil(position.z));
            return roundedPosition + oneFacingVector;
        }
    }
}

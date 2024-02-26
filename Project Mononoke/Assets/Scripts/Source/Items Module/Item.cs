using System;
using Base.Input;
using Source.BuildingModule;
using Source.Character.Movement;
using UnityEngine;

namespace Source.ItemsModule
{
    [Serializable]
    public class Item<T> : IComparable<Item<T>>, IBuildRequester where T : ItemData
    {
        public delegate void UseAction(object context);
        public UseAction OnUse;
        private Vector3 _targetBuildingPosition = Vector3.zero;

        [field: SerializeField] public int ID { get; private set;}
        [field: SerializeReference] public ItemsDatabase<T> Database { get; private set;}
        [field: SerializeField, Range(0.01f, 100f)] public float PercentsOfDurability { get; private set;}

        public Item(int iD, ItemsDatabase<T> database, int buildingID)
        {
            ID = iD;
            Database = database;
            OnUse = _ => MakeRequest(new IBuildRequester.BuildRequestEventArgs(buildingID, _targetBuildingPosition));
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
        
        public int CompareTo(Item<T> other)
        {
            return PercentsOfDurability.CompareTo(other.PercentsOfDurability);
        }

        public void MakeRequest(IBuildRequester.BuildRequestEventArgs args)
        {
            BuildingRequestsBus.MakeRequest(this, args);
        }
    }
}

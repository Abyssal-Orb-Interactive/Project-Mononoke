using System;
using Source.BuildingModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.Character
{
    public class CollidersHolder
    {
        private CharacterLogicIsometric2DCollider _logicCollider = null;
        private PickUpper _pickUpper = null;

        public event Action<object> SomethingInCollider = null;
        
        public CollidersHolder(CharacterLogicIsometric2DCollider logicCollider, PickUpper pickUpper)
        {
            _logicCollider = logicCollider;
            _pickUpper = pickUpper;
            
            logicCollider.BuildingInCollider += OnBuildingInLogicCollider;
            pickUpper.ItemPickUpped += OnItemPickUpped;
        }

        private void OnItemPickUpped(Item item)
        {
            SomethingInCollider?.Invoke(item);
        }

        private void OnBuildingInLogicCollider(Building building)
        {
            if(building.ReadyToInteract) SomethingInCollider?.Invoke(building);
        }
    }
}
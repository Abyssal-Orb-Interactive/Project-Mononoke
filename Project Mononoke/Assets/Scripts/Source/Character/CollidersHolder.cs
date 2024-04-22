using System;
using Source.BuildingModule;
using Source.PickUpModule;

namespace Source.Character
{
    public class CollidersHolder
    {
        private CharacterLogicIsometric2DCollider _logicCollider = null;
        private PickUpper _pickUpper = null;

        public event Action SomethingInCollider = null;
        
        public CollidersHolder(CharacterLogicIsometric2DCollider logicCollider, PickUpper pickUpper)
        {
            _logicCollider = logicCollider;
            _pickUpper = pickUpper;
            
            logicCollider.BuildingInCollider += OnBuildingInLogicCollider;
            pickUpper.ItemPickUpped += OnItemPickUpped;
        }

        private void OnItemPickUpped()
        {
            SomethingInCollider?.Invoke();
        }

        private void OnBuildingInLogicCollider(Building building)
        {
            if(building.ReadyToInteract) SomethingInCollider?.Invoke();
        }
    }
}
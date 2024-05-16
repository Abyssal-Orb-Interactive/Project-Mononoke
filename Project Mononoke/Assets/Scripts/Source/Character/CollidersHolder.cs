using System;
using Source.BuildingModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.Character
{
    public class CollidersHolder : MonoBehaviour
    {
        private CharacterLogicIsometric2DCollider _logicCollider = null;

        public event Action<object> SomethingInCollider = null;
        
        public void Initialize(CharacterLogicIsometric2DCollider logicCollider)
        {
            _logicCollider = logicCollider;

            _logicCollider.BuildingInCollider += OnBuildingInLogicCollider;
        }

        private void OnBuildingInLogicCollider(Building building)
        {
            if(building.ReadyToInteract) SomethingInCollider?.Invoke(building);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent<PickUpper>(out var otherPickUpper))
            {
                SomethingInCollider?.Invoke(otherPickUpper);
                return;
            }

            if (!other.gameObject.TryGetComponent<ItemView>(out var droppedItemView)) return;
            SomethingInCollider?.Invoke(droppedItemView);
        }
    }
}
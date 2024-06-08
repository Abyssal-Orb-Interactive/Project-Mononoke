using Source.Character.Minions_Manager;
using Source.InventoryModule;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    public class MinionsSpawner : Container
    {

        public override void Initialize(Inventory inventory)
        {
            base.Initialize(inventory);
        }
        
        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            MinionsFactory.Create(transform.position);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<ItemView>(out var droppedItemView))
            {
                _inventory.TryAddItem(droppedItemView.Item);
                droppedItemView.BeginPickUp();
            }
        }
    }
}
using Source.Character.Minions_Manager;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.BuildingModule.Buildings
{
    public class MinionsSpawner : Container
    {
        
        public override void StartInteractiveAction(PickUpper pickUpper)
        {
            MinionsFactory.Create(transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ItemView>(out var itemView))
            {
                _inventory.TryAddItem(itemView.Item);
                itemView.BeginPickUp();
            }
        }
    }
}
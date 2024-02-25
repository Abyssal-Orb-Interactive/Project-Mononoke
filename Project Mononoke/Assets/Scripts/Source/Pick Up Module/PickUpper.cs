using Source.InventoryModule;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
   [RequireComponent(typeof(Collider2D))]
   public class PickUpper : MonoBehaviour
   {
      public Inventory Inventory {get; private set;} = new(weightCapacity: 100, volumeCapacity: 100);
      public Hand Hand { get; private set; } = new(strength: 5, volume: 2);


      private void OnCollisionEnter2D(Collision2D other)
      {
         Inventory ??= new Inventory(100, 100);
         Hand ??= new Hand(5, 2);
         
         if(!other.gameObject.TryGetComponent<ItemView>(out var droppedItemView)) return;

         if(!Hand.TryTake(droppedItemView.Item)) return;

         droppedItemView.BeginPickUp();
      }
   }
}

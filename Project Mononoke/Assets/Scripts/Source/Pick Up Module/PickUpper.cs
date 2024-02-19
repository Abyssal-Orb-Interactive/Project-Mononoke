using Source.InventoryModule;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
   [RequireComponent(typeof(Collider2D))]
   public class PickUpper : MonoBehaviour
   {
      public Inventory Inventory {get; private set;} = new Inventory(100, 100);


      private void OnCollisionEnter2D(Collision2D other)
      {
         Inventory ??= new Inventory(100, 100);
         
         if(!other.gameObject.TryGetComponent<ItemView>(out var droppedItem)) return;

         if(!Inventory.TryAddItem(droppedItem.Item)) return;

         droppedItem.BeginPickUp();
      }
   }
}

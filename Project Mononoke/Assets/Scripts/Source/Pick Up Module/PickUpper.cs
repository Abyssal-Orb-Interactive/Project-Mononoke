using Source.InventoryModule;
using Source.InventoryModule.UI;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
   [RequireComponent(typeof(Collider2D))]
   public class PickUpper : MonoBehaviour
   {
      public Inventory Inventory {get; private set;} = new(weightCapacity: 100, volumeCapacity: 100);
      public Manipulator Manipulator { get; private set; } = new(strength: 5, volume: 2);
      private InventoryPresenter _inventoryPresenter = null;

      public void Initialize(Inventory inventory, Manipulator manipulator, InventoryPresenter presenter)
      {
         Inventory = inventory;
         Manipulator = manipulator;
         _inventoryPresenter = presenter;
         _inventoryPresenter.ItemEquipped += OnItemEquipped;
      }

      private void OnItemEquipped(InventoryPresenter.StackDataForUI itemData)
      {
         TryTakeItemFromInventoryWithManipulator(itemData.ItemID);
      }

      public bool TryTakeItemFromInventoryWithManipulator(string ID)
      {
         if (Manipulator.HasItem()) Manipulator.TryStashIn(Inventory);
         return Inventory.TryGetItem(ID, out var item) && Manipulator.TryTake(item);
      }

      public bool TryUseItemInManipulatorMatterIn(object context)
      {
         if (Manipulator == null || !Manipulator.HasItem()) return false;
         Manipulator.UseTackedItemMatterIn(context);
         return true;
      }

      public bool TryStashInInventory()
      {
         return Manipulator.TryStashIn(Inventory);
      }

      private void OnCollisionEnter2D(Collision2D other)
      {
         Inventory ??= new Inventory(100, 100);
         Manipulator ??= new Manipulator(5, 2);
         
         if (!other.gameObject.TryGetComponent<ItemView>(out var droppedItemView)) return;
         
         if(!Inventory.TryAddItem(droppedItemView.Item)) return;

         droppedItemView.BeginPickUp();
      }
   }
}

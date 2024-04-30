using System;
using Source.InventoryModule;
using Source.ItemsModule;
using UnityEngine;
using VContainer;

namespace Source.PickUpModule
{
   [RequireComponent(typeof(Collider2D))]
   public class PickUpper : MonoBehaviour
   {
      public Inventory Inventory {get; private set;} = new(weightCapacity: 100, volumeCapacity: 100);
      public Manipulator Manipulator { get; private set; } = new(strength: 5, volume: 2);
      private InventoryPresenter _inventoryPresenter = null;

      public event Action<Item> ItemPickUpped = null;

      [Inject] public void Initialize(Inventory inventory, Manipulator manipulator)
      {
         Inventory = inventory;
         Manipulator = manipulator;
      }

      public void SubscribeOnUIUpdates(InventoryPresenter presenter)
      {
         if(_inventoryPresenter != null) UnsubscribeToUIUpdates();
         _inventoryPresenter = presenter;
         _inventoryPresenter.ItemEquipped += OnItemEquipped;
      }

      public void UnsubscribeToUIUpdates()
      {
         _inventoryPresenter.ItemEquipped -= OnItemEquipped;
      }

      private void OnItemEquipped(InventoryPresenter.StackDataForUI itemData)
      {
         TryTakeItemFromInventoryWithManipulator(itemData.ItemData.ID);
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

      public bool TryStashInPickUpperInventory()
      {
          return  Manipulator.TryStashIn(Inventory);
      }

      public bool TryGiveTo(Inventory inventory)
      {
         return Manipulator.TryStashIn(inventory);
      }

      private void OnCollisionEnter2D(Collision2D other)
      {
         Inventory ??= new Inventory(100, 100);
         Manipulator ??= new Manipulator(5, 2);

         if (other.gameObject.TryGetComponent<PickUpper>(out var otherPickUpper))
         {
            TryGiveTo(otherPickUpper.Inventory);
            return;
         }
         
         if (!other.gameObject.TryGetComponent<ItemView>(out var droppedItemView)) return;
         if(!Inventory.TryAddItem(droppedItemView.Item)) return;
         ItemPickUpped?.Invoke(droppedItemView.Item);
         droppedItemView.BeginPickUp();
      }
   }
}

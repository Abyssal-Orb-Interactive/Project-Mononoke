using System;
using Source.Character;
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
      private CollidersHolder _collidersHolder = null;

      public event Action<Item> ItemPickUpped = null;

      [Inject]
      public void Initialize(Inventory inventory, Manipulator manipulator, CollidersHolder collidersHolder)
      {
         Inventory = inventory;
         Manipulator = manipulator;
         _collidersHolder = collidersHolder;
         _collidersHolder.SomethingInCollider += ProcessCollision;
      }

      private void ProcessCollision(object something)
      {
         Inventory ??= new Inventory();
         Manipulator ??= new Manipulator(5, 2);

         switch (something)
         {
            case PickUpper pickUpper:
               TryGiveTo(pickUpper.Inventory);
               return;
            case ItemView itemView:
               if(!Inventory.TryAddItem(itemView.Item)) return;
               ItemPickUpped?.Invoke(itemView.Item);
               itemView.BeginPickUp();
               return;
         }
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
   }
}

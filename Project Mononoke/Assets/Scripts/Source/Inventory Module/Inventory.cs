using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Source.ItemsModule;
using UnityEngine;
using static Source.InventoryModule.InventoryItemsStackFabric;
using static Source.ItemsModule.TrashItemsDatabaseSO;

namespace Source.InventoryModule
{
    public class Inventory : IEnumerable<InventoryItemsStack>
    {
      private float _weightCapacity = 0;
      private float _volumeCapacity = 0;
      private float _availableWeight = 0;
      private float _availableVolume = 0;

      private Dictionary<int, List<InventoryItemsStack>> _inventory = null; 

      public int Count => _inventory.Keys.Count;

      public event Action<InventoryItem> ItemAdded, ItemRemoved, ItemDropped;

      public Inventory(float weightCapacity, float volumeCapacity)
      {
        _weightCapacity = weightCapacity;
        _volumeCapacity = volumeCapacity;
        _availableWeight = _weightCapacity;
        _availableVolume = _volumeCapacity;
        _inventory = new(30);
      } 

      public bool TryAddItem(IPickUpable item)
      {
        return TryAddItem(new InventoryItem(item.ID, item.Database));
      }

      public bool TryAddItem(InventoryItem item)
        {
          if (EnterAddingParametersIsInvalid(item) || CantGetItemDataFrom(item.Database, item.ID, out ItemData itemData)) return false;

          if (ItemDoesNotFitsInInventoryBy(itemData.Weight, itemData.Volume)) return false;

          if (InventoryDoesNotContainsStacksOf(item.ID, out List<InventoryItemsStack> stacks))
          {
            if (CantAddRecordForStacksBy(item.ID, out stacks)) return false;
          }

          var stackForAdding = GetFirstIncompleteStackOrDefault(stacks);
          if (NoIncompleteStacks(stackForAdding))
            {
                if (CantAddStack(itemData, stacks)) return false;
                stackForAdding = stacks.Last();
            }

            if (StackCantAddItem(item, stackForAdding)) return false;

          DecreaseAvailableWeightAndVolumeUsing(itemData);
          ItemAdded?.Invoke(item);
          return true;
        }

      private bool CantAddStack(ItemData itemData, List<InventoryItemsStack> stacks)
      {
        return !TryAddStack(itemData, stacks);
      }

      private bool EnterAddingParametersIsInvalid(InventoryItem item)
      {
        return item.Equals(default) || item.Database == null || _inventory == null;
      }

      private bool CantGetItemDataFrom(IPickUpableDatabase database, int ID, out ItemData itemData)
      {
        return !database.TryGetItemDataBy(ID, out itemData);
      }

      private bool ItemDoesNotFitsInInventoryBy(float weight, float volume)
      {
        return weight > _availableWeight || volume > _availableVolume;
      }

      private bool InventoryDoesNotContainsStacksOf(int ID, out List<InventoryItemsStack> stacks)
      {
        return !_inventory.TryGetValue(ID, out stacks);
      }

      private bool CantAddRecordForStacksBy(int ID, out List<InventoryItemsStack> stacks)
      {
        return !_inventory.TryAdd(ID, stacks = new List<InventoryItemsStack>());
      }

      private InventoryItemsStack GetFirstIncompleteStackOrDefault(List<InventoryItemsStack> stacks)
      {
        return stacks.FirstOrDefault(stack => stack.IsFull() == false);
      }

      private bool StackCantAddItem(InventoryItem item, InventoryItemsStack stackForAdding)
      {
        return !stackForAdding.TryPushItem(item);
      }

      private bool NoIncompleteStacks(InventoryItemsStack stackForAdding)
      {
        return stackForAdding == null;
      }

      private bool TryAddStack(ItemData itemData, List<InventoryItemsStack> stacks)
      {
        if (InventoryItemsStackFabricCantCreateNewStack(stackIndex: stacks.Count, out InventoryItemsStack newStack, itemData.MaxStackCapacity)) return false;
        stacks.Add(newStack);
        return true;
      }

      private static bool InventoryItemsStackFabricCantCreateNewStack(int stackIndex, out InventoryItemsStack newStack, int stackCapacity)
      {
        return !InventoryItemsStackFabric.TryCreate(stackIndex, out newStack, stackCapacity);
      }

        private void DecreaseAvailableWeightAndVolumeUsing(ItemData itemData)
      {
        _availableWeight -= itemData.Weight;
        _availableVolume -= itemData.Volume;
      }

      public bool TryGetItem(int itemID, int stackIndex, out InventoryItem item)
      {
        if (EnterGettingParametersIsInvalid(itemID, stackIndex))
        {
          item = default;
          return false;
        }

        if (InventoryDoesNotContainsStacksOf(itemID, out List<InventoryItemsStack> stacks))
        {
          item = default;
          return false;
        }
        
        if (StackIndexIsOutOfBoundsOfStacksList(stackIndex, stacks.Count))
        {
          item = default;
          return false;
        }

        var stack = stacks[stackIndex];

        if (StackCantPopItem(out item, stack)) return false;

        ItemRemoved?.Invoke(item);
        return true;
      }

      private static bool EnterGettingParametersIsInvalid(int itemID, int stackIndex)
      {
        return itemID < 0 || stackIndex < 0;
      }

      private bool StackIndexIsOutOfBoundsOfStacksList(int stackIndex, int stacksCount)
      {
        return stackIndex >= stacksCount;
      }

      private bool StackCantPopItem(out InventoryItem item, InventoryItemsStack stack)
      {
        return !stack.TryPopItem(out item);
      }

      public IEnumerator<InventoryItemsStack> GetEnumerator()
      {
        for(var key = 0; key < _inventory.Keys.Count; key++)
        {
          for(var stackIndex = 0; stackIndex < _inventory[key].Count; stackIndex++) yield return _inventory[key][stackIndex];
        }
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }

      public readonly struct InventoryItem : IComparable<InventoryItem>
      {
        public int ID { get; }
        public IPickUpableDatabase Database { get; }
        public float PercentsOfDurability { get; }

        public InventoryItem(int iD, IPickUpableDatabase database) : this()
            {
                ID = iD;
                Database = database;
            }

        public readonly int CompareTo(InventoryItem other)
        {
          return PercentsOfDurability.CompareTo(other.PercentsOfDurability);
        }
      }
    }
}

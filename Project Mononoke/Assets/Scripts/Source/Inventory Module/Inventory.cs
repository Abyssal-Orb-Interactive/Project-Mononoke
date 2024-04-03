using System;
using System.Collections;
using System.Collections.Generic;
using Source.ItemsModule;
using static Source.InventoryModule.ItemsStackFabric;

namespace Source.InventoryModule
{
    public class Inventory : IEnumerable<InventoryItemsStack>
    {
      private float _weightCapacity = 0;
      private float _volumeCapacity = 0;
      private float _availableWeight = 0;
      private float _availableVolume = 0;

      private Dictionary<string, List<InventoryItemsStack>> _inventory = null; 

      public int Count => _inventory.Keys.Count;

      public event Action<InventoryItemsStack, int> ItemAdded, ItemDropped;
      public event Action<InventoryItemsStack, int, Item> ItemRemoved;

      public Inventory(float weightCapacity, float volumeCapacity)
      {
        _weightCapacity = weightCapacity;
        _volumeCapacity = volumeCapacity;
        _availableWeight = _weightCapacity;
        _availableVolume = _volumeCapacity;
        _inventory = new Dictionary<string, List<InventoryItemsStack>>(30);
      }

      public bool TryAddItem(Item item)
        {
          if (EnterAddingParametersIsInvalid(item) || CantGetItemDataFrom(item.Database, item.ID, out var itemData)) return false;

          if (ItemDoesNotFitsInInventoryBy(itemData.Weight, itemData.Volume)) return false;

          if (InventoryDoesNotContainsStacksOf(item.ID, out var stacks))
          {
            if (CantAddRecordForStacksBy(item.ID, out stacks)) return false;
          }

          var indexOfStackForAdding = GetFirstIncompleteStackOrDefault(stacks);
          if (NoIncompleteStacks(indexOfStackForAdding))
          {
            if (CantAddStack(itemData, stacks)) return false;
            indexOfStackForAdding = stacks.Count - 1;
          }

          var stackForAdding = stacks[indexOfStackForAdding];
          if (StackCantAddItem(item, stackForAdding)) return false;

          DecreaseAvailableWeightAndVolumeUsing(itemData);      
          ItemAdded?.Invoke(stackForAdding, indexOfStackForAdding);
          return true;
        }

      private bool CantAddStack(IItemData itemData, ICollection<InventoryItemsStack> stacks)
      {
        return !TryAddStack(itemData, stacks);
      }

      private bool EnterAddingParametersIsInvalid(Item item)
      {
        return item.Equals(default) || item.Database == null || _inventory == null;
      }

      private bool CantGetItemDataFrom(ItemsDatabase database, string ID, out IItemData itemData)
      {
        return !database.TryGetItemDataBy(ID, out itemData);
      }

      private bool ItemDoesNotFitsInInventoryBy(float weight, float volume)
      {
        return weight > _availableWeight || volume > _availableVolume;
      }

      private bool InventoryDoesNotContainsStacksOf(string ID, out List<InventoryItemsStack> stacks)
      {
        return !_inventory.TryGetValue(ID, out stacks);
      }

      private bool CantAddRecordForStacksBy(string ID, out List<InventoryItemsStack> stacks)
      {
        return !_inventory.TryAdd(ID, stacks = new List<InventoryItemsStack>());
      }

      private int GetFirstIncompleteStackOrDefault(List<InventoryItemsStack> stacks)
      {
        return stacks.FindIndex(stack => stack.IsFull() == false);
      }

      private bool StackCantAddItem(Item item, InventoryItemsStack stackForAdding)
      {
        return !stackForAdding.TryPushItem(item);
      }

      private bool NoIncompleteStacks(int stackIndex)
      {
        return stackIndex == -1;
      }

      private bool TryAddStack(IItemData itemData, ICollection<InventoryItemsStack> stacks)
      {
        if (InventoryItemsStackFabricCantCreateNewStack(stackIndex: stacks.Count, out InventoryItemsStack newStack, itemData.MaxStackCapacity)) return false;
        stacks.Add(newStack);
        return true;
      }

      private static bool InventoryItemsStackFabricCantCreateNewStack(int stackIndex, out InventoryItemsStack newStack, int stackCapacity)
      {
        return !ItemsStackFabric.TryCreate(stackIndex, out newStack, stackCapacity);
      }

        private void DecreaseAvailableWeightAndVolumeUsing(IItemData itemData)
      {
        _availableWeight -= itemData.Weight;
        _availableVolume -= itemData.Volume;
      }

      public bool TryGetItem(string itemID, int stackIndex, out Item item)
      {
        if (EnterGettingParametersIsInvalid(stackIndex))
        {
          item = default;
          return false;
        }

        if (InventoryDoesNotContainsStacksOf(itemID, out var stacks))
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

        ItemRemoved?.Invoke(stack, stackIndex, item);
        return true;
      }
      
      public bool TryGetItem(string itemID, out Item item)
      {
        if (InventoryDoesNotContainsStacksOf(itemID, out var stacks))
        {
          item = default;
          return false;
        }

        var stack = stacks[0];

        if (StackCantPopItem(out item, stack)) return false;

        ItemRemoved?.Invoke(stack, 0, item);
        return true;
      }

      private static bool EnterGettingParametersIsInvalid(int stackIndex)
      {
        return stackIndex < 0;
      }

      private bool StackIndexIsOutOfBoundsOfStacksList(int stackIndex, int stacksCount)
      {
        return stackIndex >= stacksCount;
      }

      private bool StackCantPopItem(out Item item, InventoryItemsStack stack)
      {
        return !stack.TryPopItem(out item);
      }

      public IEnumerator<InventoryItemsStack> GetEnumerator()
      {
        throw new NotImplementedException();
      }

      IEnumerator IEnumerable.GetEnumerator()
      {
        return GetEnumerator();
      }
    }
}

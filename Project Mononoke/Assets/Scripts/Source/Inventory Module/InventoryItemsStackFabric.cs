using Base.DataStructuresModule;
using static Source.InventoryModule.Inventory;

namespace Source.InventoryModule
{
    public static class InventoryItemsStackFabric
    {
        public static bool TryCreate(int stackIndex, out InventoryItemsStack stack, int stackCapacity = 1)
        {
            if(stackIndex < 0 || stackCapacity < 1)
            {
                stack = null;
                return false;
            }
            var queue = new PriorityQueue<InventoryItem>(stackCapacity);
            stack = new InventoryItemsStack(stackIndex, queue);
            return true;
        }

        public class InventoryItemsStack
        {
            public int StackIndex { get; }
            private readonly PriorityQueue<InventoryItem> _stack;

            public InventoryItemsStack(int stackIndex, PriorityQueue<InventoryItem> queue)
            {
                if(stackIndex < 0 || queue == null) return;
                StackIndex = stackIndex;
                _stack = queue;
            }

            public bool TryPushItem(InventoryItem item)
            {
              return _stack.TryEnqueue(item);
            }

            public bool IsFull()
            {
                return _stack.Count == _stack.Capacity;
            }

            public bool TryPeekItem(out InventoryItem item)
            {
                return _stack.TryPeek(out item);
            }

            public bool IsEmpty()
            {
                return _stack.Count == 0;
            }

            public bool TryPopItem(out InventoryItem item)
            {
                return _stack.TryDequeue(out item);
            }
        }
    }
}

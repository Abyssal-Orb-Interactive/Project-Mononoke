using Base.DataStructuresModule;
using Source.ItemsModule;

namespace Source.InventoryModule
{
    public static class ItemsStackFabric
    {
        public static bool TryCreate(int stackIndex, out InventoryItemsStack stack, int stackCapacity = 1)
        {
            if(stackIndex < 0 || stackCapacity < 1)
            {
                stack = null;
                return false;
            }
            var queue = new PriorityQueue<Item>();
            stack = new InventoryItemsStack(stackIndex, queue);
            return true;
        }

        public class InventoryItemsStack
        {
            public int StackIndex { get; }
            private readonly PriorityQueue<Item> _stack;

            public int Count => _stack.Count;

            public InventoryItemsStack(int stackIndex, PriorityQueue<Item> queue)
            {
                if(stackIndex < 0 || queue == null) return;
                StackIndex = stackIndex;
                _stack = queue;
            }

            public bool TryPushItem(Item item)
            {
              return _stack.TryEnqueue(item);
            }

            public bool IsFull()
            {
                return _stack.Count == _stack.Capacity;
            }

            public bool TryPeekItem(out Item item)
            {
                return _stack.TryPeek(out item);
            }

            public bool IsEmpty()
            {
                return _stack.Count == 0;
            }

            public bool TryPopItem(out Item item)
            {
                return _stack.TryDequeue(out item);
            }
        }
    }
}

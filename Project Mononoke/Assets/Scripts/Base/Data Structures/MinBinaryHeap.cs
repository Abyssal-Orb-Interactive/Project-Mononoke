using System;
using System.Collections;
using System.Collections.Generic;

namespace Base.DataStructuresModule
{
    public class MinBinaryHeap<T> : IEnumerable<T>, IReadOnlyCollection<T>, IDisposable where T : IComparable<T>
    {
        private const int MIN_HEAP_CAPACITY = 1;

        private readonly T[] _heap = null;
        public int Count { get; private set; } = 0;
        public int Capacity => _heap.Length;

        public MinBinaryHeap(int capacity = MIN_HEAP_CAPACITY)
        {
            _heap = capacity < MIN_HEAP_CAPACITY ? new T[MIN_HEAP_CAPACITY] : new T[capacity];

            Count = 0;
        }

        public bool TryInsert(T comparable)
        {
            if(Count == Capacity) return false;
            Insert(comparable);
            return true;
        }

        private void Insert(T comparable)
        {
            _heap[Count] = comparable;
            Count++;
            HeapifyUp();
        }

        public bool TryExtractMin(out T comparable)
        {
            if(Count == 0)
            {
                comparable = default;
                return false;
            }
            
            comparable = ExtractMin();
            return true;
        }

        private T ExtractMin()
        {
            var minObject = _heap[0];
            var lastIndex = Count - 1;
            
            Swap(0, lastIndex);
            Count--;
            HeapifyDown();

            _heap[lastIndex] = default;
            return minObject;
        }

        public bool TryPeekMin(out T comparable)
        {
            if(Count == 0) 
            {
                comparable = default;
                return false;
            }
            
            comparable = PeekMin();
            return true;
        }

        private T PeekMin()
        {
            return _heap[0];
        }

        public void Clear()
        {
            Array.Clear(_heap, 0, Count);
            Count = 0;
        }

        public bool Contains(T comparable)
        {
            return Count != 0 && ContainsRecursive(0, comparable);
        }

        private bool ContainsRecursive(int currentIndex, T comparable)
        {
            if (currentIndex >= Count) return false;

            if (_heap[currentIndex].Equals(comparable)) return true;

            var comparison = _heap[currentIndex].CompareTo(comparable);

            if (comparison >= 0) return false;
            
            return ContainsRecursive(2 * currentIndex + 1, comparable) || 
                   ContainsRecursive(2 * currentIndex + 2, comparable);
        }

        private void HeapifyUp()
        {
            var currentIndex = Count - 1;

            while(currentIndex > 0)
            {
                var parentIndex = (currentIndex - 1) / 2;

                if(_heap[currentIndex].CompareTo(_heap[parentIndex]) < 0) 
                {
                    Swap(currentIndex, parentIndex);
                    currentIndex = parentIndex;
                }
                else break;
            }
        }

        private void HeapifyDown()
        {
            var currentIndex = 0;
            var lastIndex = Count - 1;
            var smallestIndex = currentIndex;

            while(true)
            {
                var leftChildIndex = 2 * currentIndex + 1;
                var rightChildIndex = 2 * currentIndex + 2;

                if(leftChildIndex <= lastIndex && _heap[leftChildIndex].CompareTo(_heap[smallestIndex]) < 0)
                {
                    smallestIndex = leftChildIndex;
                }

                if(rightChildIndex <= lastIndex && _heap[rightChildIndex].CompareTo(_heap[smallestIndex]) < 0)
                {
                    smallestIndex = rightChildIndex;

                }

                if(smallestIndex != currentIndex)
                {
                    Swap(currentIndex, smallestIndex);
                    currentIndex = smallestIndex;
                }
                else 
                {
                    break;
                }
            }
        }

        private void Swap(int index1, int index2)
        {
            (_heap[index1], _heap[index2]) = (_heap[index2], _heap[index1]);
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return _heap[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            Clear();
            GC.SuppressFinalize(this);
        }
    }
}

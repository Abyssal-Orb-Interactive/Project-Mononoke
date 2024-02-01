using System;
using System.Collections;
using System.Collections.Generic;

namespace Base.DataStructuresModule
{
    public class PriorityQueue<T> : IEnumerable<T>, IReadOnlyCollection<T>, IDisposable where T : IComparable<T>
    {
        private const int MIN_QUEUE_CAPACITY = 1;

        private readonly MinBinaryHeap<T> _queue = null;

        public int Count => _queue.Count;
        public int Capacity => _queue.Capacity;


        public PriorityQueue(int capacity = MIN_QUEUE_CAPACITY)
        {
            if(capacity < MIN_QUEUE_CAPACITY) _queue = new MinBinaryHeap<T>(MIN_QUEUE_CAPACITY);
            else _queue = new MinBinaryHeap<T>(capacity);   
        }

        public bool TryEnqueue(T comparable)
        {
            return _queue.TryInsert(comparable);
        }

        public bool TryDequeue(out T comparable)
        {
            return _queue.TryExtractMin(out comparable);
        }

        public bool TryPeek(out T comparable)
        {
            return _queue.TryPeekMin(out comparable);
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public bool Contains(T comparable)
        {
            return _queue.Contains(comparable);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _queue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
           _queue.Dispose();
           GC.SuppressFinalize(this);
        }
    }
}

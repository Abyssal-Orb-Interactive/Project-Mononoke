using System;

namespace Base.Math
{
    public interface IAddable<T> : IComparable<T>
    {
        public T Add(T other);
    }
}
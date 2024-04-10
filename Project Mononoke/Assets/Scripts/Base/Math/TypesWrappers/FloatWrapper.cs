using System.Globalization;
using UnityEngine;

namespace Base.Math.TypesWrappers
{
    public struct FloatWrapper : IAddable<FloatWrapper>
    {
        public float Value { get; private set; }

        public FloatWrapper(float value)
        {
            Value = value;
        }
        
        public int CompareTo(FloatWrapper other)
        {
           return Value.CompareTo(other.Value);
        }

        public FloatWrapper Add(FloatWrapper other)
        {
            Value += other.Value;
            return this;
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
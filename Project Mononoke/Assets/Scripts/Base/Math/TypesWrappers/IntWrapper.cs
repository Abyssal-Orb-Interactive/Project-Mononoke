using System.Globalization;

namespace Base.Math.TypesWrappers
{
    public class IntWrapper : IAddable<IntWrapper>
    {
        public int Value { get; private set; }
        
        public IntWrapper(int value)
        {
            Value = value;
        }
        
        public int CompareTo(IntWrapper other)
        {
            return Value.CompareTo(other.Value);
        }

        public IntWrapper Add(IntWrapper other)
        {
            return new IntWrapper(Value + other.Value);
        }
        
        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
using System;

namespace Base.Math
{
    public static class FunctionNatureDeterminant
    {
        public static NatureOfFunction DeterminateFunctionNature<T>(Func<T> function) where T : IAddable<T>
        {
            var startValue = function();
            var nextValue = startValue;
            nextValue.Add(function());
            return  nextValue.CompareTo(startValue) switch
            {
                0 => NatureOfFunction.Constant,
                > 0 => NatureOfFunction.Increasing,
                < 0 => NatureOfFunction.Decreasing
            };
        }
    }
}
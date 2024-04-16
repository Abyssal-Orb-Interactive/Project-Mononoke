using System;
using Base.Math;
using UnityEngine;

namespace Base.Counters
{
    public class Counter<T> : IDisposable where T : IAddable<T>
    {
        public T StartValue { get; private set; } = default;
        public T TargetValue { get; private set; } = default;
        public T CurrentValue { get; private set; } = default;
        public NatureOfFunction NatureOfCounting { get; private set; } = NatureOfFunction.Constant;

        private Func<T> _countingFunction = null;

        public event Action TargetReached = null;
        public event Action ValueChanged = null;

        public Counter(T startValue, T targetValue, Func<T> countingFunction)
        {

            _countingFunction = countingFunction;
            NatureOfCounting = FunctionNatureDeterminant.DeterminateFunctionNature(_countingFunction);
            TargetValue = targetValue;
            StartValue = startValue;
            Reset();
            if (IsCounterInvalid()) throw new ArgumentException("Values of this counter is invalid");
        }

        private bool IsCounterInvalid()
        {
             var comparingResult = StartValue.CompareTo(TargetValue);
             return _countingFunction == null ||
                    (NatureOfCounting == NatureOfFunction.Increasing && !(comparingResult < 0)) ||
                    (NatureOfCounting == NatureOfFunction.Decreasing && !(comparingResult > 0)) ||
                    StartValue == null || TargetValue == null || NatureOfCounting == NatureOfFunction.Constant;

        }

        public void CalculateNextValue()
        {
            CurrentValue = CurrentValue.Add(_countingFunction());
            OnValueChanged();
            if (CurrentValue.CompareTo(TargetValue) == 0) OnTargetReached();
            if (IsCounterInvalid()) OnTargetReached();
            
           
        }

        public void Reset()
        {
                CurrentValue = StartValue;
                OnValueChanged();
        }

        public bool TryChangeTargetValueOn(T newValue)
        {

            var oldValueBuffer = TargetValue;
            ChangeTargetValueOn(newValue);

            if (!IsCounterInvalid()) return true;

            TargetValue = oldValueBuffer;
            return false;
        }

        private void ChangeTargetValueOn(T newValue)
        {

            TargetValue = newValue;

        }

        private void OnTargetReached()
        {
            
                TargetReached?.Invoke();
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke();
        }

        public void Dispose()
        {
            Reset();
            StartValue = default;
            CurrentValue = default;
            TargetValue = default;
            _countingFunction = null;
            TargetReached = null;
            ValueChanged = null;
            GC.SuppressFinalize(this);
        }
    }
}
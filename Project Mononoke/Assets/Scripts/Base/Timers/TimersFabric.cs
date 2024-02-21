using System;
using UnityEngine;
using VContainer;

namespace Base.Timers
{
    public static class TimersFabric
    {
        private static TimeInvoker _invoker = null;

        [Inject]
        public static void Initialize(TimeInvoker invoker)
        {
            _invoker = invoker;
        }

        public static Timer Create(Timer.TimerType type, float delayTimeInSeconds)
        {
            if (_invoker == null) 
                throw new ArgumentException("Before creating timers, you must initialize TimerFabric");
            
            return type switch
            {
                Timer.TimerType.ScaledSecond => new Timer(delayTimeInSeconds, () => _invoker.ONE_SECOND, _invoker,
                    type),
                Timer.TimerType.UnscaledSecond => new Timer(delayTimeInSeconds, () => _invoker.ONE_SECOND, _invoker,
                    type),
                Timer.TimerType.ScaledFrame => new Timer(delayTimeInSeconds, () => _invoker.GetDeltaTime(), _invoker,
                    type),
                Timer.TimerType.UnscaledFrame => new Timer(delayTimeInSeconds, () => _invoker.GetUnscaledDeltaTime(),
                    _invoker, type),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
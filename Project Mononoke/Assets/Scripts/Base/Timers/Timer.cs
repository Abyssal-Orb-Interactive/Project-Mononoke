using System;
using Base.Counters;
using Base.Math.TypesWrappers;
using UnityEngine;

namespace Base.Timers
{
    public delegate void TimerAction();
    
    public class Timer : IDisposable
    {
        public enum TimerType
        {
            ScaledSecond = 0,
            UnscaledSecond = 1,
            ScaledFrame = 2,
            UnscaledFrame = 3
        }
        
        private readonly Counter<FloatWrapper> _counter = null;
        private TimeInvoker _timeInvoker = null;
        private readonly TimerType _type = TimerType.ScaledSecond;

        public event TimerAction TimerFinished = null;
        public event TimerAction TimerPaused = null;
        public event TimerAction TimerResumed = null;
        public event TimerAction TimerTicked = null;

        public float ElapsedTime => _counter.CurrentValue.Value;
        public float DelayTimeInSeconds => _counter.TargetValue.Value;
        public float RemainingTime => DelayTimeInSeconds - ElapsedTime;

        public bool IsPaused { get; protected set; } = true;

        public Timer(float delayTimeInSeconds, Func<float> timeSource, TimeInvoker invoker, TimerType type)
        {
            if(invoker == null) throw new ArgumentNullException(nameof(invoker), "TimeInvoker cannot be null.");;
            
            _counter = new Counter<FloatWrapper>(new FloatWrapper(0f), new FloatWrapper(delayTimeInSeconds), () => new FloatWrapper(timeSource()));
            _counter.TargetReached += OnTimerEnds;
            _timeInvoker = invoker;
            _type = type;
        }

        public void Start()
        {
           Resume();
        }

        public void Stop()
        {
            Pause();
            _counter.Reset();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Pause()
        {
            OnTimerPaused();
        }

        public void Resume()
        {
            OnTimerResumed();
        }

        private void StartListenTimeUpdates()
        {
            switch (_type)
            {
                case TimerType.ScaledSecond:
                    _timeInvoker.SecondUpdated += OnTimerTick;
                    return;
                case TimerType.UnscaledSecond:
                    _timeInvoker.UnscaledFrameUpdated += OnTimerTick;
                    return;
                case TimerType.ScaledFrame:
                    _timeInvoker.FrameUpdated += OnTimerTick;
                    return;
                case TimerType.UnscaledFrame:
                    _timeInvoker.UnscaledFrameUpdated += OnTimerTick;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void StopListenTimeUpdates()
        {
            switch (_type)
            {
                case TimerType.ScaledSecond:
                    _timeInvoker.SecondUpdated -= OnTimerTick;
                    return;
                case TimerType.UnscaledSecond:
                    _timeInvoker.UnscaledFrameUpdated -= OnTimerTick;
                    return;
                case TimerType.ScaledFrame:
                    _timeInvoker.FrameUpdated -= OnTimerTick;
                    return;
                case TimerType.UnscaledFrame:
                    _timeInvoker.UnscaledFrameUpdated -= OnTimerTick;
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnTimerEnds()
        {
            Stop();
            TimerFinished?.Invoke();
        }
        
        private void OnTimerPaused()
        {
            if(IsPaused) return;
            IsPaused = true;
            StopListenTimeUpdates();
            TimerPaused?.Invoke();
        }
        
        private void OnTimerResumed()
        {
            if(!IsPaused) return;
            IsPaused = false;
            StartListenTimeUpdates();
            TimerResumed?.Invoke();
        }
        
        private void OnTimerTick()
        {
            _counter.CalculateNextValue();
            TimerTicked?.Invoke();
        }

        public void Dispose()
        {
            Stop();
            _counter.Dispose();
            _timeInvoker = null;
            TimerFinished = null;
            TimerPaused = null;
            TimerResumed = null;
            TimerTicked = null;
            GC.SuppressFinalize(this);
        }
    }
}
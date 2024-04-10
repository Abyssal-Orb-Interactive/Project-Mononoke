using System;
using UnityEngine;

namespace Base.Timers
{
    public delegate void FrameTimeAction();
    public delegate void UnitTimeAction();

    public class TimeInvoker : MonoBehaviour
    {
        public float ONE_SECOND = 1f;
        
        
        public event FrameTimeAction FrameUpdated;
        public event FrameTimeAction UnscaledFrameUpdated;
        public event UnitTimeAction SecondUpdated;
        public event UnitTimeAction UnscaledSecondUpdated;

        private float _oneSecondTime;
        private float _unscaledOneSecondTime;

        private static Lazy<TimeInvoker> _instance = new Lazy<TimeInvoker>(() => CreateInstance());
        public static TimeInvoker Instance => _instance.Value;
        
        private static TimeInvoker CreateInstance()
        {
            var gameObject = new GameObject("Time Invoker");
            var invoker = gameObject.AddComponent<TimeInvoker>();
            DontDestroyOnLoad(gameObject);
            return invoker;
        }
        
        public void UpdateTimer()
        {
            var deltaTime = GetDeltaTime();
            FrameUpdated?.Invoke();

            var timeScale = GetTimeScale();
            _oneSecondTime += deltaTime;
            if (_oneSecondTime >= ONE_SECOND)
            {
                _oneSecondTime -= ONE_SECOND;
                SecondUpdated?.Invoke();
            }

            var unscaledDeltaTime = GetUnscaledDeltaTime();
            UnscaledFrameUpdated?.Invoke();

            _unscaledOneSecondTime += unscaledDeltaTime;
            
            if (_unscaledOneSecondTime < ONE_SECOND) return;
            
            _unscaledOneSecondTime -= ONE_SECOND;
            UnscaledSecondUpdated?.Invoke();
        }

        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public float GetUnscaledDeltaTime()
        {
            return Time.unscaledDeltaTime;
        }

        public float GetTimeScale()
        {
            return Time.timeScale;
        }

        private void OnDestroy()
        {
            FrameUpdated = null;
            SecondUpdated = null;
            UnscaledFrameUpdated = null;
            UnscaledSecondUpdated = null;
            _instance = null;
        }
    }
}
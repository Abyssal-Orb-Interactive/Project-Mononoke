using System.Collections.Generic;
using Base.Timers;
using UnityEngine;

namespace Source.PickUpModule
{
    public static class FallingCurveVertexesGenerator
    {
        private static AnimationCurve _curveTemplate = null;
        private static float _durationInSeconds = 0;
        private static float _yHeight = 0;
        private static bool _isTimerEnds = false;

        public static void Initialize(AnimationCurve curveTemplate, float durationInSeconds, float yHeight)
        {
            _curveTemplate = curveTemplate;
            _durationInSeconds = durationInSeconds;
            _yHeight = yHeight;
        }

        public static IEnumerable<Vector3> GetCurveVertexes()
        {
            var vertices = new List<Vector3>();
            var timer = TimersFabric.Create(Timer.TimerType.ScaledFrame, _durationInSeconds);
            _isTimerEnds = false;
            timer.TimerFinished += OnTimerFinished;

            while (!_isTimerEnds)
            {
                var templateY = _curveTemplate.Evaluate(timer.ElapsedTimeInPresents);
                var realY = templateY * _yHeight;
                vertices.Add(new Vector3(0f, realY));
            }

            return vertices;
        }

        private static void OnTimerFinished()
        {
            _isTimerEnds = true;
        }
    }
}
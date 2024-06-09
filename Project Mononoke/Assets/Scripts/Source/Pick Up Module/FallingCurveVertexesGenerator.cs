using System.Collections.Generic;
using Base.Timers;
using UnityEngine;

namespace Source.PickUpModule
{
    public class FallingCurveVertexesGenerator
    {
        private AnimationCurve _curveTemplate = null;
        private float _durationInSeconds = 0;


        public FallingCurveVertexesGenerator(AnimationCurve curveTemplate, float durationInSeconds)
        {
            _curveTemplate = curveTemplate;
            _durationInSeconds = durationInSeconds;
        }

        public IEnumerable<Vector3> GetCurveVertexesBetween(Vector3 startPosition, Vector3 targetPosition, float yHeight)
        {
            var vertexes = new List<Vector3>();
            var elapsedTime = 0f;
            var timeStep = Time.fixedDeltaTime;

            while (elapsedTime < _durationInSeconds)
            {
                var t = elapsedTime / _durationInSeconds;
                var templateY = _curveTemplate.Evaluate(t);
                var realY = templateY * yHeight;
                var position = Vector3.Lerp(startPosition, targetPosition, t);
                position.y += realY;
                vertexes.Add(position);
                elapsedTime += timeStep;
            }

            return vertexes;
        }
    }
}
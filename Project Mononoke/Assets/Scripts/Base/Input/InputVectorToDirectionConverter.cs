using System;
using UnityEngine;

namespace Base.Input
{
    public static class InputVectorToDirectionConverter
    {
        private const float THRESHOLD = 0.5f;
        private const float FULL_CIRCLE_DEGREES = 360f;
        private const float ZERO_ANGLE = 0f;
        
        private static readonly int DirectionsCount = Enum.GetValues(typeof(MovementDirection)).Length;
        private static readonly float DegreesPerDirection = FULL_CIRCLE_DEGREES / DirectionsCount;

        public static MovementDirection GetMovementDirectionFor(Vector2 directionVector)
        {
            var angleFromXAxis = CalculateAngleFor(directionVector);

            if (ZeroAngleBiggerThan(angleFromXAxis)) Normalize(ref angleFromXAxis);

           
            var segmentIndex = CalculateIndexOfDirectionsCircleSegmentFor(angleFromXAxis);
            return (MovementDirection)segmentIndex;
        }

        private static bool ThresholdBiggerThen(Vector2 directionVector)
        {
            return directionVector.magnitude < THRESHOLD;
        }

        private static float CalculateAngleFor(Vector2 directionVector)
        {
            return Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg;
        
        }
        
        private static bool ZeroAngleBiggerThan(float angleFromXAxis)
        {
            return angleFromXAxis < ZERO_ANGLE;
        }
        
        private static void Normalize(ref float angleFromXAxis)
        {
            angleFromXAxis += FULL_CIRCLE_DEGREES;
        }
        
        private static int CalculateIndexOfDirectionsCircleSegmentFor(float angle)
        {
            return Mathf.FloorToInt(angle / DegreesPerDirection) % DirectionsCount;
        }
    }
}
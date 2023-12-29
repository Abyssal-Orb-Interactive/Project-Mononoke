using System;
using UnityEngine;

namespace Base.Input
{
    public static class InputVectorToDirectionConverter
    {
        private const float THRESHOLD = 0.5f;
        private const float FULL_CIRCLE_DEGREES = 360f;
        private const float SEGMENT_HALF = 0.5f;
        private const float ZERO_ANGLE = 0f;
        
        private static readonly int DIRECTIONS_COUNT = Enum.GetValues(typeof(MovementDirection)).Length - 1; // !WARNING: Minus 1 because Enum contains direction Stay. 
        private static readonly float DEGREES_PER_DIRECTION = FULL_CIRCLE_DEGREES / DIRECTIONS_COUNT;

        public static MovementDirection GetMovementDirectionFor(Vector2 directionVector)
        {
            if (ThresholdBiggerThen(directionVector)) return MovementDirection.Stay;

            float angleFromXAxis = CalculateAngleFor(directionVector);

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
            return Mathf.FloorToInt(angle / DEGREES_PER_DIRECTION) % DIRECTIONS_COUNT;
        }
    }
}
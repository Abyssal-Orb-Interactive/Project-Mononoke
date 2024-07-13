using System;
using System.Collections.Generic;
using Base.Input;
using UnityEngine;

namespace Source.BattleSystem
{
    [CreateAssetMenu(fileName = "CircleDamageArea", menuName = "DamageAreas/Circle")]
    [Serializable]
    public class IsometricCircleDamageArea : DamageAreaTemplate
    {
        private const float CIRCLE_DEGREES = 360f;
        [field: SerializeField] public float Radius { get; private set; } = 5f;
        [field: SerializeField] public int NumberOfSegments { get; private set; } = 10;

        public override IEnumerable<Vector2> GetVertices(MovementDirection facing)
        {
            var points = new Vector2[NumberOfSegments];
            var angleStep = CIRCLE_DEGREES / NumberOfSegments;

            for (var i = 0; i < NumberOfSegments; i++)
            {
                var rad = Mathf.Deg2Rad * (angleStep * i);
                points[i] = new Vector2(
                    Radius * Mathf.Cos(rad),
                    Radius * Mathf.Sin(rad) * 0.5f
                );
            }
            
            
            return points;
        }
    }
}
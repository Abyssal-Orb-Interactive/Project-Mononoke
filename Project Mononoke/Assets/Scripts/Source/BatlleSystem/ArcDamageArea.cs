using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.BattleSystem
{
    [CreateAssetMenu(fileName = "ArcDamageArea", menuName = "DamageAreas/Arc")]
    [Serializable]
    public class ArcDamageArea : DamageAreaTemplate
    {
        [field: SerializeField] public float Radius { get; private set; } = 5f;
        [field: SerializeField] public float StartAngle { get; private set; } = 0f;
        [field: SerializeField] public float EndAngle { get; private set; } = 90f;
        [field: SerializeField] public int NumberOfSegments { get; private set; } = 10;

        private List<Vector2> _vertices = null;

        private void OnValidate()
        {
            _vertices = CalculateVertices();
        }

        public override IEnumerable<Vector2> GetVertices()
        {
            _vertices ??= CalculateVertices();
            
            return _vertices;
        }
        
        private List<Vector2> CalculateVertices()
        {
            var vertices = new List<Vector2>();
            var angleIncrement = (EndAngle - StartAngle) / NumberOfSegments;
            
            vertices.Add(Vector2.zero);

            for (var i = 0; i <= NumberOfSegments; i++)
            {
                var angle = StartAngle + i * angleIncrement;
                var radian = angle * Mathf.Deg2Rad;
                var vertex = new Vector2(Mathf.Cos(radian) * Radius, Mathf.Sin(radian) * Radius);
                vertices.Add(vertex);
            }
            
            vertices.Add(Vector2.zero);
            return vertices;
        }
    }
}
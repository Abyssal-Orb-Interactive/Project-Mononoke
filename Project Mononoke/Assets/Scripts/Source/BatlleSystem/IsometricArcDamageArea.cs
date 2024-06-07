using System;
using System.Collections.Generic;
using Base.Input;
using Base.Math;
using UnityEngine;

namespace Source.BattleSystem
{
    [CreateAssetMenu(fileName = "ArcDamageArea", menuName = "DamageAreas/Arc")]
    [Serializable]
    public class IsometricArcDamageArea : DamageAreaTemplate
    {
        [field: SerializeField] public float Radius { get; private set; } = 5f;
        [field: SerializeField] public float StartAngle { get; private set; } = 0f;
        [field: SerializeField] public float EndAngle { get; private set; } = 90f;
        [field: SerializeField] public int NumberOfSegments { get; private set; } = 10;
        [field: SerializeField] public MovementDirection Facing { get; private set; } = MovementDirection.East;
        
        private List<Vector2> _vertices = null;

        private void OnValidate()
        {
            CalculateVerticesWith(Facing);
        }

        public override IEnumerable<Vector2> GetVertices()
        {
            CalculateVerticesWith(Facing);
            return _vertices;
        }
        
        private void CalculateVerticesWith(MovementDirection facing)
        {
            if (_vertices == null)
                _vertices = new List<Vector2>();
            else
                _vertices.Clear();
            
            var angleIncrement = (EndAngle - StartAngle) / NumberOfSegments;
            var rotationAngle = DirectionToQuaternionConverter.GetQuaternionFor(facing).eulerAngles.z;

            _vertices.Add(Vector2.zero);

            for (var i = 0; i <= NumberOfSegments; i++)
            {
                var angle = StartAngle + i * angleIncrement;
                var radian = angle * Mathf.Deg2Rad;
                var x = Mathf.Cos(radian) * Radius;
                var y = Mathf.Sin(radian) * Radius;

                // Rotate the vertices based on the current rotation angle
                var rotatedX = x * Mathf.Cos(rotationAngle) - y * Mathf.Sin(rotationAngle);
                var rotatedY = x * Mathf.Sin(rotationAngle) + y * Mathf.Cos(rotationAngle);
                var vertex = new Vector2(rotatedX, rotatedY);
                var isoVertex = new Vector2Iso(vertex);
                _vertices.Add(new Vector2(isoVertex.X, isoVertex.Y));
            }
            
            _vertices.Add(Vector2.zero);
        }
    }
}
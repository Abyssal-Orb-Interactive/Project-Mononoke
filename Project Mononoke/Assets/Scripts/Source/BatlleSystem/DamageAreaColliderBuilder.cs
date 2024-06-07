using System.Collections.Generic;
using System.Linq;
using Base.Input;
using Base.Math;
using Source.Character.Movement;
using UnityEngine;

namespace Source.BattleSystem
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class DamageAreaColliderBuilder : MonoBehaviour
    {
        [SerializeField] private DamageAreaTemplate _areaTemplate = null;
        [SerializeField] private float _weaponLenght = 1f;

        private MovementDirection _facing = MovementDirection.NorthWest;

        private void Start()
        {
            BuildCollider();
        }

        private void BuildCollider()
        {
            if(_areaTemplate == null) return;
            
            var vertices = _areaTemplate.GetVertices();
            var polygonalCollider = GetComponent<PolygonCollider2D>();

            polygonalCollider.SetPath(0, vertices.Select(vertex => new Vector2(vertex.x * _weaponLenght, vertex.y * _weaponLenght)).ToArray());
        }
    }
}
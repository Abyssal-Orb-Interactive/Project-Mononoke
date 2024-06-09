using System.Linq;
using Base.Input;
using Source.Character.Movement;
using UnityEngine;

namespace Source.BattleSystem
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class DamageAreaColliderBuilder : MonoBehaviour
    {
        [SerializeField] private DamageAreaTemplate _areaTemplate = null;
        [SerializeField] private float _weaponLenght = 1f;
        private IsoCharacterMover _characterMover = null;

        private MovementDirection _facing = MovementDirection.NorthWest;

        private void Start()
        {
            BuildCollider();
            _characterMover = transform.gameObject.GetComponentInParent<IsoCharacterMover>();
            _characterMover.MovementChanged += OnMovementChanged;
        }

        private void OnMovementChanged(object sender, IsoCharacterMover.MovementActionEventArgs e)
        {
            if(_facing == e.Facing) return;

            _facing = e.Facing;
            BuildCollider();
        }

        private void BuildCollider()
        {
            if(_areaTemplate == null) return;
            
            var vertices = _areaTemplate.GetVertices(_facing);
            var polygonalCollider = GetComponent<PolygonCollider2D>();

            polygonalCollider.SetPath(0, vertices.Select(vertex => new Vector2(vertex.x * _weaponLenght, vertex.y * _weaponLenght)).ToArray());
        }
    }
}
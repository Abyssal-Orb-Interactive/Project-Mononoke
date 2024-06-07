using System.Linq;
using UnityEngine;

namespace Source.BattleSystem
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class DamageAreaColliderBuilder : MonoBehaviour
    {
        [SerializeField] private DamageAreaTemplate _areaTemplate = null;

        private void Start()
        {
            BuildCollider();
        }

        private void BuildCollider()
        {
            if(_areaTemplate == null) return;
            var vertices = _areaTemplate.GetVertices();
            var polygonalCollider = GetComponent<PolygonCollider2D>();
            polygonalCollider.SetPath(0, vertices.ToArray());
        }
    }
}
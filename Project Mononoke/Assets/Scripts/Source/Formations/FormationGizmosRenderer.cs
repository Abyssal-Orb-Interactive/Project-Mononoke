using UnityEngine;

namespace Source.Formations
{
    [RequireComponent(typeof(Formation))]
    public class FormationGizmosRenderer : MonoBehaviour
    {
        [SerializeField] private Vector3 _unitGizmosSizes = Vector3.one;
        [SerializeField] private Color _gizmosColor = Color.green;
        private Formation _formation = null;

        private void OnValidate()
        {
            _formation ??= GetComponent<Formation>();
        }

        private void OnDrawGizmos()
        {
            if(_formation == null) return;
            Gizmos.color = _gizmosColor;
            foreach (var position in _formation.GetFormationPositions())
            {
                Gizmos.DrawCube(transform.position + position + new Vector3(0, _unitGizmosSizes.y * 0.5f, 0),
                    _unitGizmosSizes);
            }
        }
    }
}
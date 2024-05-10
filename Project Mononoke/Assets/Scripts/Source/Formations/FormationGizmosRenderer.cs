using Base.Math;
using UnityEngine;
using VContainer;

namespace Source.Formations
{
    public class FormationGizmosRenderer : MonoBehaviour
    {
        [SerializeField] private Vector3 _unitGizmosSizes = Vector3.one;
        [SerializeField] private Color _gizmosColor = Color.green;
        private Formation _formation = null;

        [Inject]
        public void Initialize(Formation formation)
        {
            _formation = formation;
        }

        private void OnDrawGizmos()
        {
            if(_formation == null) return;
            Gizmos.color = _gizmosColor;
            foreach (var position in _formation.GetFormationPositions())
            {
                var isoPos = new Vector3Iso(position);
                Gizmos.DrawCube(new Vector3(isoPos.X, isoPos.Y, isoPos.Z) + new Vector3(0, _unitGizmosSizes.y * 0.5f, 0),
                    _unitGizmosSizes);
            }
        }
    }
}
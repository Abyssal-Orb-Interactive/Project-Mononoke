using UnityEngine;

namespace Source.Items
{
    public class Hoe : MonoBehaviour
    {
       [SerializeField] private GameObject _seedBedPrefab = null;
       [SerializeField] private Vector3 _targetPosition = Vector3.zero;


        [ContextMenu ("Plow")]
        public void Plow()
        {
            Instantiate(_seedBedPrefab, _targetPosition, Quaternion.identity);
        }
    }
}

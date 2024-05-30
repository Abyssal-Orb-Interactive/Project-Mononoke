using UnityEngine;

namespace Source.BattleSystem.UI
{
    public class HealthBarsCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _healthBarPrefab = null;

        public void AddHealthBarTo(StatsHolder statsHolder)
        {
            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, statsHolder.transform.position);
            var healthBarObject = Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity, transform);
            var healthBarObjectRectTransform = healthBarObject.GetComponent<RectTransform>();
            healthBarObjectRectTransform.SetParent(transform, false);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPosition, Camera.main, out var localPoint);
            healthBarObjectRectTransform.localPosition = localPoint;
            healthBarObjectRectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            var healthBar = healthBarObject.GetComponentInChildren<HealthBar>();
            healthBar.Initialize(statsHolder);
        }
    }
}
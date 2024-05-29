using Source.BuildingModule;
using UnityEngine;

namespace Source.BattleSystem.UI
{
    public class HealthBarsCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _healthBarPrefab = null;
        [SerializeField] private OnGridObjectPlacer _objectPlacer = null;

        public void AddHealthBarTo(StatsHolder statsHolder)
        {
            
            var healthBarObject = _objectPlacer.PlaceObject(new ObjectPlacementInformation<GameObject>(_healthBarPrefab, statsHolder.transform.position, Quaternion.identity, transform));
            var healthBarObjectRectTransform = healthBarObject.GetComponent<RectTransform>();
            healthBarObjectRectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            var healthBar = healthBarObject.GetComponentInChildren<HealthBar>();
            healthBar.Initialize(statsHolder);
        }
    }
}
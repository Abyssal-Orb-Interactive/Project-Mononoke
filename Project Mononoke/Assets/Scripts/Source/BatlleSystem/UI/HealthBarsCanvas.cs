using System.Collections.Generic;
using Source.Character.Movement;
using UnityEngine;

namespace Source.BattleSystem.UI
{
    public class HealthBarsCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject _healthBarPrefab = null;
        private Dictionary<IsoCharacterMover, RectTransform> _spawnedHealthBarsForMobileObjects = null;

        public void AddHealthBarTo(StatsHolder statsHolder)
        {
            SpawnHealthBarObject(out var healthBarObjectRectTransform);

            UpdateHealthBarInCanvasPosition(statsHolder.transform.position, healthBarObjectRectTransform);

            if (statsHolder.TryGetComponent<IsoCharacterMover>(out var mover))
            {
                _spawnedHealthBarsForMobileObjects ??= new Dictionary<IsoCharacterMover, RectTransform>();
                _spawnedHealthBarsForMobileObjects.Add(mover, healthBarObjectRectTransform);
                mover.MovementChanged += MovementChanged;
            }

            healthBarObjectRectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            
            InitializeHealthBarWith(statsHolder, healthBarObjectRectTransform.gameObject.GetComponentInChildren<HealthBar>());
        }

        private void MovementChanged(object sender, IsoCharacterMover.MovementActionEventArgs e)
        {
            var mover = (IsoCharacterMover)sender;
            UpdateHealthBarInCanvasPosition(mover.transform.position, _spawnedHealthBarsForMobileObjects[mover]);
        }

        private void UpdateHealthBarInCanvasPosition(Vector3 worldPosition, RectTransform healthBarObjectRectTransform)
        {
            var inCanvasHealthBarPosition = CalculateHealthBarsLocalCanvasPosition(worldPosition);
            healthBarObjectRectTransform.position = inCanvasHealthBarPosition;
        }

        private GameObject SpawnHealthBarObject(out RectTransform healthBarObjectRectTransform)
        {
            var healthBarObject = Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity, transform);
            healthBarObjectRectTransform = healthBarObject.GetComponent<RectTransform>();
            healthBarObjectRectTransform.SetParent(transform, false);
            return healthBarObject;
        }

        private static void InitializeHealthBarWith(StatsHolder statsHolder, HealthBar healthBar)
        {
            healthBar.Initialize(statsHolder);
        }

        private Vector3 CalculateHealthBarsLocalCanvasPosition(Vector3 worldPosition)
        {
            var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPosition, Camera.main,
                out var localPoint);
            return localPoint;
        }
    }
}
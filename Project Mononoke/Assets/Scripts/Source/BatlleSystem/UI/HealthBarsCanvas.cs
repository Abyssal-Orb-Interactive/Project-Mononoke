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
            var healthBarObject = SpawnHealthBarObject(out var healthBarObjectRectTransform);
            if (statsHolder.TryGetComponent<IsoCharacterMover>(out var mover))
            {
                _spawnedHealthBarsForMobileObjects ??= new Dictionary<IsoCharacterMover, RectTransform>();
                _spawnedHealthBarsForMobileObjects.Add(mover, healthBarObjectRectTransform);
                mover.MovementChanged += MovementChanged;
            }

            var localPoint = CalculateInCanvasLocalPositionUsing(statsHolder.transform.position);

            SetHealthBarsLocalPositionAndScale(healthBarObjectRectTransform, localPoint);
            InitializeHealthBarWith(statsHolder, healthBarObject.GetComponentInChildren<HealthBar>());
        }
        
        private GameObject SpawnHealthBarObject(out RectTransform healthBarObjectRectTransform)
        {
            var healthBarObject = Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity, transform);
            healthBarObjectRectTransform = healthBarObject.GetComponent<RectTransform>();
            return healthBarObject;
        }

        private Vector2 CalculateInCanvasLocalPositionUsing(Vector3 worldPosition)
        {
            var uppedWorldPosition = worldPosition + new Vector3(0, 0.5f, 0);
            var screenPosition = CalculateInScreenPositionFor(uppedWorldPosition);
            var localPoint = CalculateInCanvasLocalPositionFor(screenPosition);
            return localPoint;
        }

        private static Vector2 CalculateInScreenPositionFor(Vector3 worldPosition)
        {
            var screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);
            return screenPosition;
        }

        private Vector2 CalculateInCanvasLocalPositionFor(Vector2 screenPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, screenPosition,
                Camera.main, out var localPoint);
            return localPoint;
        }

        private static void SetHealthBarsLocalPositionAndScale(RectTransform healthBarObjectRectTransform,
            Vector2 localPoint)
        {
            SetHealthBarLocalPosition(healthBarObjectRectTransform, localPoint);
            healthBarObjectRectTransform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }

        private static void SetHealthBarLocalPosition(RectTransform healthBarObjectRectTransform, Vector2 localPoint)
        {
            healthBarObjectRectTransform.localPosition = localPoint;
        }

        private void MovementChanged(object sender, IsoCharacterMover.MovementActionEventArgs e)
        {
            var mover = (IsoCharacterMover)sender;
            UpdateHealthBarPositionUsing(mover);
        }

        private void UpdateHealthBarPositionUsing(IsoCharacterMover mover)
        {
            var localPoint = CalculateInCanvasLocalPositionUsing(mover.transform.position);
            SetHealthBarLocalPosition(_spawnedHealthBarsForMobileObjects[mover], localPoint);
        }

        private static void InitializeHealthBarWith(StatsHolder statsHolder, HealthBar healthBar)
        {
            healthBar.Initialize(statsHolder);
        }
    }
}
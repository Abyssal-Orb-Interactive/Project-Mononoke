using System.Collections.Generic;
using Source.Character.Movement;
using UnityEngine;
using VContainer;

namespace Source.BattleSystem.UI
{
    public class HealthBarsCanvas : MonoBehaviour
    {
        private GameObject _healthBarPrefab = null;
        private GameObject _surroundingHealthBarPrefab = null;
        private Dictionary<IsoCharacterMover, RectTransform> _spawnedHealthBarsForMobileObjects = null;
        private Dictionary<Damageable, RectTransform> _spawnedHealthBarsForStaticObjects = null;

        [Inject] public void Initialize(GameObject healthBarPrefab, GameObject surroundingHealthBar)
        {
            _healthBarPrefab = healthBarPrefab;
            _surroundingHealthBarPrefab = surroundingHealthBar;
            _spawnedHealthBarsForMobileObjects = new Dictionary<IsoCharacterMover, RectTransform>();
            _spawnedHealthBarsForStaticObjects = new Dictionary<Damageable, RectTransform>();
        }

        public void UpdateAllHealthBarsPositions()
        {
            foreach (var kvp in _spawnedHealthBarsForMobileObjects)
            {
                UpdateHealthBarPositionUsing(kvp.Key);
            }
        }

        public void AddHealthBarTo(Damageable damageable)
        {
            var healthBarObject = SpawnHealthBarObject(damageable, out var healthBarObjectRectTransform);
            
            if (!damageable.TryGetComponent<IsoCharacterMover>(out var mover))
            {
                _spawnedHealthBarsForStaticObjects.Add(damageable, healthBarObjectRectTransform);
                return;
            }
            
            _spawnedHealthBarsForMobileObjects.Add(mover, healthBarObjectRectTransform);
            mover.MovementChanged += MovementChanged;
        }

        public void AddHealthBarTo(EnemySurroundingFunnel surroundingFunnel)
        {
            var damageable = surroundingFunnel.Surrounded as Damageable;
            if(damageable == null) return;
            var healthBarObject =
                SpawnSurroundingHealthBarObject(damageable, out var healthBarObjectRectTransform);

            var counter = healthBarObject.GetComponentInChildren<SurroundersCounter>();
            if (counter != null)
            {
                counter.Initialize(surroundingFunnel);
            }
            
            if (!damageable.TryGetComponent<IsoCharacterMover>(out var mover))
            {
                if (!_spawnedHealthBarsForStaticObjects.ContainsKey(damageable))
                    _spawnedHealthBarsForStaticObjects.Remove(damageable);
                _spawnedHealthBarsForStaticObjects.Add(damageable, healthBarObjectRectTransform);
                return;
            }

            if (!_spawnedHealthBarsForMobileObjects.ContainsKey(mover))
            {
                mover.MovementChanged -= MovementChanged;
                _spawnedHealthBarsForMobileObjects.Remove(mover);
            }
            _spawnedHealthBarsForMobileObjects.Add(mover, healthBarObjectRectTransform);
            mover.MovementChanged += MovementChanged;
        }

        private GameObject SpawnHealthBarObject(Damageable damageable, out RectTransform healthBarObjectRectTransform)
        {
            var healthBarObject = Instantiate(_healthBarPrefab, Vector3.zero, Quaternion.identity, transform);
            healthBarObjectRectTransform = healthBarObject.GetComponent<RectTransform>();
            ResizeHealthBar(damageable, healthBarObjectRectTransform);
            InitializeHealthBarWith(damageable, healthBarObject.GetComponentInChildren<HealthBar>());
            return healthBarObject;
        }
        private GameObject SpawnSurroundingHealthBarObject(Damageable damageable, out RectTransform healthBarObjectRectTransform)
        {
            var healthBarObject = Instantiate(_surroundingHealthBarPrefab, Vector3.zero, Quaternion.identity, transform);
            healthBarObjectRectTransform = healthBarObject.GetComponent<RectTransform>();
            ResizeHealthBar(damageable, healthBarObjectRectTransform);
            InitializeHealthBarWith(damageable, healthBarObject.GetComponentInChildren<HealthBar>());
            return healthBarObject;
        }

        private void ResizeHealthBar(Damageable damageable, RectTransform healthBarObjectRectTransform)
        {
            if (!damageable.TryGetComponent<SpriteRenderer>(out var spriteRenderer)) return;
            var spriteSize = spriteRenderer.bounds.size;
            healthBarObjectRectTransform.sizeDelta = new Vector2(spriteSize.x, healthBarObjectRectTransform.sizeDelta.y);
        }
        
        private static void InitializeHealthBarWith(Damageable damageable, HealthBar healthBar)
        {
            healthBar.Initialize(damageable);
        }
        
        private void MovementChanged(object sender, IsoCharacterMover.MovementActionEventArgs e)
        {
            var mover = (IsoCharacterMover)sender;
            UpdateHealthBarPositionUsing(mover);
        }

        private void UpdateHealthBarPositionUsing(IsoCharacterMover mover)
        {
            if (_spawnedHealthBarsForMobileObjects.TryGetValue(mover, out var healthBarRectTransform))
            {
                UpdateHealthBarPositionUsing(mover.transform, healthBarRectTransform);
            }
        }

        private void UpdateHealthBarPositionUsing(Transform target, RectTransform healthBarRectTransform)
        {
            var worldPosition = target.position + new Vector3(0, 0.5f, 0); // Adjust the height offset as needed
            healthBarRectTransform.position = worldPosition;
        }
    }
}
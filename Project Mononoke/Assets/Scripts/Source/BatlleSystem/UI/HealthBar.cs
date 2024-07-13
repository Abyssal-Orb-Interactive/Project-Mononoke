using UnityEngine;
using UnityEngine.UI;

namespace Source.BattleSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable = null;
        [SerializeField] private Image _barSprite = null;
        [SerializeField] private GameObject _healthBarVisual = null;

        private void OnValidate()
        {
            _barSprite ??= GetComponent<Image>();
        }

        public void Initialize(Damageable damageable)
        {
            _damageable = damageable;
            _damageable.HealthPointsChanged += OnHealthPointsChanging;
            _damageable.Death += OnDeath;
            _healthBarVisual = gameObject.transform.parent.parent.gameObject;
        }

        private void OnDeath(IDamageable damageable)
        {
            _healthBarVisual.SetActive(false);
        }

        private void OnHealthPointsChanging(float percentsOfHealth)
        {
            _barSprite.fillAmount = percentsOfHealth;
        }
    }
}
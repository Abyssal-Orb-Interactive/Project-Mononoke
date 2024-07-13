using UnityEngine;
using UnityEngine.UI;

namespace Source.BattleSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Damageable _damageable = null;
        [SerializeField] private Image _barSprite = null;

        private void OnValidate()
        {
            _barSprite ??= GetComponent<Image>();
        }

        public void Initialize(Damageable damageable)
        {
            _damageable = damageable;
            _damageable.HealthPointsChanged += OnHealthPointsChanging;
        }

        private void OnHealthPointsChanging(float percentsOfHealth)
        {
            _barSprite.fillAmount = percentsOfHealth;
        }
    }
}
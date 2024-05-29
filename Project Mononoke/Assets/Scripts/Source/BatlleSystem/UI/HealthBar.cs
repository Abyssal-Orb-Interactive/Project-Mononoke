using UnityEngine;
using UnityEngine.UI;

namespace Source.BattleSystem.UI
{
    [RequireComponent(typeof(Image))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private StatsHolder _statsHolder = null;
        [SerializeField] private Image _barSprite = null;

        private void OnValidate()
        {
            _barSprite ??= GetComponent<Image>();
        }

        public void Initialize(StatsHolder statsHolder)
        {
            _statsHolder = statsHolder;
            _statsHolder.HealthPointsChanged += OnHealthPointsChanging;
        }

        private void OnHealthPointsChanging(float percentsOfHealth)
        {
            _barSprite.fillAmount = percentsOfHealth;
        }
    }
}
using Source.BattleSystem;
using UnityEngine;

namespace Source.Character.Visual.UI
{
    public class HealthBar : MonoBehaviour
    {
        private StatsHolder _statsHolder = null;
        private SpriteRenderer _barSprite = null;

        public void Initialize(StatsHolder statsHolder, SpriteRenderer spriteRenderer)
        {
            _statsHolder = statsHolder;
            _barSprite = spriteRenderer;
            _statsHolder.HealthPointsChanged += OnHealthPointsChanging;
        }

        private void OnHealthPointsChanging(float percentsOfHealth)
        {
            //_barSprite.
        }
    }
}
using Base.Input;
using Base.Math;
using Source.BattleSystem;
using Source.BattleSystem.UI;
using UnityEngine;

namespace Source.Character.AI.BattleAI.Behaviours.EnemyInDamageAreBehaviours
{
    public class HitWithSurroundingBehaviour : HitBehaviour
    {
        private EnemySurroundingFunnel _surroundingFunnel = null;
        private HealthBarsCanvas _healthBarsCanvas = null;

        public HitWithSurroundingBehaviour(IDamager damager, HealthBarsCanvas healthBarsCanvas) : base(damager)
        {
            _healthBarsCanvas = healthBarsCanvas;
        }

        public override void Execute(IDamageable enemy)
        {
            if(enemy.CurrentHealthPointsInPercents <= 0f) return;
            if(enemy.Fraction == _damager.Fraction) return;
            if (_surroundingFunnel == null)
            {
                _surroundingFunnel = new EnemySurroundingFunnel(enemy, _damager);
                _healthBarsCanvas.AddHealthBarTo(_surroundingFunnel);
            }
            enemy.Death += OnEnemyDeath;
            enemy.TakeDamage(_damager);
        }

        private void OnEnemyDeath(IDamageable enemy)
        {
            _surroundingFunnel = null;
        }
    }
}
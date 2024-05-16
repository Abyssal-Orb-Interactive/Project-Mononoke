using Source.Character;
using UnityEngine;

namespace Source.BattleSystem
{
    public class StatsHolder : MonoBehaviour, IDamageable, IDamager
    {
        private float _healthPoints = 0f;
        private float _unarmedAttackDamage = 0f;
        public Fractions Fraction { get; private set; } = Fractions.Plodomorphs;

        public void Initialize(float healthPoints, float unarmedAttackDamage, Fractions fraction)
        {
            _healthPoints = healthPoints;
            _unarmedAttackDamage = unarmedAttackDamage;
            Fraction = fraction;
        }
        
        public void TakeDamage(IDamager damageSource)
        {
            _healthPoints -= damageSource.GetDamage();
        }

        public float GetDamage()
        {
            return _unarmedAttackDamage;
        }
    }
}
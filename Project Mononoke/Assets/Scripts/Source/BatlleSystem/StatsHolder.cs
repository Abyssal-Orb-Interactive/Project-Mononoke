using System;
using Source.Character;
using UnityEngine;

namespace Source.BattleSystem
{
    public class StatsHolder : MonoBehaviour, IDamageable, IDamager
    {
        private float _maxHealthPoints = 0f;
        private float _unarmedAttackDamage = 0f;
        private float _currentHealthPoints = 0f;
        public Fractions Fraction { get; private set; } = Fractions.Plodomorphs;
        public float CurrentHealthPointsInPercents => _currentHealthPoints / _maxHealthPoints;

        public event Action<StatsHolder> EntityDead = null; 

        public void Initialize(float healthPoints, float unarmedAttackDamage, Fractions fraction)
        {
            _maxHealthPoints = healthPoints;
            _currentHealthPoints = _maxHealthPoints;
            _unarmedAttackDamage = unarmedAttackDamage;
            Fraction = fraction;
        }
        
        public void TakeDamage(IDamager damageSource)
        {
            if (_currentHealthPoints <= 0)
            {
                EntityDead?.Invoke(this);
                gameObject.SetActive(false);
                return;
            }
            _currentHealthPoints -= damageSource.GetDamage();
            if (_currentHealthPoints <= 0)
            {
                EntityDead?.Invoke(this);
                gameObject.SetActive(false);
            }
        }

        public float GetDamage()
        {
            return _unarmedAttackDamage;
        }
    }
}
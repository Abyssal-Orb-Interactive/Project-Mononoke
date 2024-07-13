using System;
using UnityEngine;

namespace Source.BattleSystem
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        private float _maxHealthPoints = 0f;
        private float _currentHealthPoints = 0f;
        
        public float CurrentHealthPointsInPercents => _currentHealthPoints / _maxHealthPoints;
        public Fractions Fraction { get; private set; }

        public event Action<IDamageable> Death;
        public event Action<float> HealthPointsChanged = null;
        
        public void Initialize(float healthPoints, Fractions fraction)
        {
            _maxHealthPoints = healthPoints;
            _currentHealthPoints = _maxHealthPoints;
            Fraction = fraction;
        }

        public void TakeDamage(IDamager damageSource)
        {
            if (_currentHealthPoints <= 0)
            {
                return;
            }
            _currentHealthPoints -= damageSource.GetDamage();
            HealthPointsChanged?.Invoke(CurrentHealthPointsInPercents);
            
            if (_currentHealthPoints <= 0)
            {
                Death?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }
}
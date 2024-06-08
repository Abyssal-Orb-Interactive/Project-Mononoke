using System;
using UnityEngine;

namespace Source.BattleSystem
{
    public class Damageable : MonoBehaviour, IDamageable
    {
        private float _maxHealthPoints = 0f;
        private float _currentHealthPoints = 0f;
        
        public float CurrentHealthPointsInPercents => _currentHealthPoints / _maxHealthPoints;
        
        public event Action<Damageable> EntityDead = null;
        public event Action<float> HealthPointsChanged = null;
        
        public void Initialize(float healthPoints)
        {
            _maxHealthPoints = healthPoints;
            _currentHealthPoints = _maxHealthPoints;
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
            HealthPointsChanged?.Invoke(CurrentHealthPointsInPercents);
            if (_currentHealthPoints <= 0)
            {
                EntityDead?.Invoke(this);
                gameObject.SetActive(false);
            }
        }
    }
}
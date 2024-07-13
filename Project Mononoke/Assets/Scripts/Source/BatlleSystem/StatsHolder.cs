using System;
using UnityEngine;

namespace Source.BattleSystem
{
    public class StatsHolder : MonoBehaviour, IDamager
    {
        private float _unarmedAttackDamage = 0f;
        private Damageable _damageable = null;
        public Fractions Fraction { get; private set; } = Fractions.Neutral;
        public event Action<IDamager> Attack = null;
        public event Action<Damageable> EntityDead = null;  

        public void Initialize(Damageable damageable, float unarmedAttackDamage, Fractions fraction)
        {
            _damageable = damageable;
            _damageable.Death += OnDamageableDeath;
            _unarmedAttackDamage = unarmedAttackDamage;
            Fraction = fraction;
        }

        private void OnDamageableDeath(IDamageable damageable)
        {
            EntityDead?.Invoke((Damageable)damageable);
        }

        public void TriggerAttack()
        {
            Attack?.Invoke(this);
        }

        public float GetDamage()
        {
            return _unarmedAttackDamage;
        }
    }
}
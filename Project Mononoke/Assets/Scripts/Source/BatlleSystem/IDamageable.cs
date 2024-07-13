using System;

namespace Source.BattleSystem
{
    public interface IDamageable
    {
        public event Action<IDamageable> Death;
        public void TakeDamage(IDamager damageSource);
        public float CurrentHealthPointsInPercents { get; }
        public Fractions Fraction { get; }
    }
}
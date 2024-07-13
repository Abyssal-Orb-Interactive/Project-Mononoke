using System;

namespace Source.BattleSystem
{
    public interface IDamager
    {
        public event Action<IDamager> Attack;
        public float GetDamage();
        public Fractions Fraction { get; }
    }
}
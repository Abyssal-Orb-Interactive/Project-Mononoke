namespace Source.BattleSystem
{
    public interface IDamageable
    {
        public void TakeDamage(IDamager damageSource);
        public float CurrentHealthPointsInPercents { get; }
    }
}
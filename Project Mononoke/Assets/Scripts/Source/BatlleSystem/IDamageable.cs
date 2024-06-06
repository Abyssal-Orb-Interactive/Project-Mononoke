using Source.Character;

namespace Source.BattleSystem
{
    public interface IDamageable
    {
        public void TakeDamage(IDamager damageSource);
    }
}
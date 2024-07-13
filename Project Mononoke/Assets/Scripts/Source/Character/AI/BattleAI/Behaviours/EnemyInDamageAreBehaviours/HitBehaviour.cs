using Source.BattleSystem;

namespace Source.Character.AI.BattleAI.Behaviours.EnemyInDamageAreBehaviours
{
    public class HitBehaviour : IEnemyInDamageAreaBehaviour
    {
        protected IDamager _damager = null;

        public HitBehaviour(IDamager damager)
        {
            _damager = damager;
        }
        
        public virtual void Execute(IDamageable enemy)
        {
            enemy.TakeDamage(_damager);
        }
    }
}
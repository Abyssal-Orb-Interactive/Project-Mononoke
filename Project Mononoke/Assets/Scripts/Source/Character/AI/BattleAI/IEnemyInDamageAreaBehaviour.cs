using Source.BattleSystem;

namespace Source.Character.AI.BattleAI
{
    public interface IEnemyInDamageAreaBehaviour
    {
        public void Execute(IDamageable enemy);
    }
}
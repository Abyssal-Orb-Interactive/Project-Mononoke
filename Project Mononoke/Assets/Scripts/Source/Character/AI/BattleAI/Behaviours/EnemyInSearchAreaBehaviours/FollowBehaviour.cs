using Source.BattleSystem;
using UnityEngine;

namespace Source.Character.AI.BattleAI.Behaviours.EnemyInSearchAreaBehaviours
{
    public class FollowBehaviour : IEnemyInSearchAreaBehaviour
    {
        private readonly IDamager _damager = null;
        private readonly PathfinderAI _pathfinderAI = null;
        
        public FollowBehaviour(IDamager damager, PathfinderAI pathfinderAI)
        {
            _damager = damager;
            _pathfinderAI = pathfinderAI;
        }
        
        public void Execute(IDamager enemy)
        {
            var component = enemy as MonoBehaviour;
            if(component == null) return;
            
            if (_damager.Fraction == enemy.Fraction || enemy.Fraction == Fractions.Neutral) return;
            
            Follow(component.transform.position);
        }

        private void Follow(Vector3 enemyPosition)
        {
            _pathfinderAI.StartFollowingPath(enemyPosition);
        }
    }
}
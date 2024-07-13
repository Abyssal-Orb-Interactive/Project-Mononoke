using Source.BattleSystem;
using UnityEngine;

namespace Source.Character.AI.BattleAI.Behaviours.EnemyInSearchAreaBehaviours
{
    public class KiteBehaviour : IEnemyInSearchAreaBehaviour
    {
        private readonly IDamager _damager = null;
        private readonly PathfinderAI _pathfinderAI = null;

        public KiteBehaviour(IDamager damager, PathfinderAI pathfinderAI)
        {
            _damager = damager;
            _pathfinderAI = pathfinderAI;
        }
        
        public void Execute(IDamager enemy)
        {
            var component = enemy as MonoBehaviour;
            if (component == null) return;
            
            if (_damager.Fraction == enemy.Fraction) return;

            Kite(component.transform.position);
        }

        private void Kite(Vector3 enemyPosition)
        {
            var aiPosition =  _pathfinderAI.transform.position;
            var directionToEnemy = enemyPosition - aiPosition;
            var oppositeDirection = -directionToEnemy.normalized;
            var kitingPosition = aiPosition + oppositeDirection;
            _pathfinderAI.StartFollowingPath(kitingPosition);
        }
    }
}
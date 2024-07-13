using Base.Input;
using Base.Math;
using Source.BattleSystem;
using UnityEngine;

namespace Source.Character.AI.BattleAI.Behaviours.EnemyInDamageAreBehaviours
{
    public class ShootBehaviour : IEnemyInDamageAreaBehaviour
    {
        private ProjectileLauncher _projectileLauncher = null;
        private PathfinderAI _pathfinderAI = null;
        private IDamager _damager = null;

        public ShootBehaviour(ProjectileLauncher projectileLauncher, PathfinderAI pathfinderAI, IDamager damager)
        {
            _projectileLauncher = projectileLauncher;
            _pathfinderAI = pathfinderAI;
            _damager = damager;
        }
        

        public void Execute(IDamageable enemy)
        {
            if (enemy.Fraction == _damager.Fraction) return;

            var component = enemy as MonoBehaviour;
            if(component == null) return;

            var enemyWorldPosition = component.transform.position;
            var enemyIsoVectorPos = new Vector3Iso(enemyWorldPosition.x, enemyWorldPosition.y, enemyWorldPosition.z);
            var enemyCartesianPos = enemyIsoVectorPos.ToCartesian();

            var shooterWorldPos = _pathfinderAI.transform.position;
            var shooterVectorPos = new Vector3Iso(shooterWorldPos.x, shooterWorldPos.y, shooterWorldPos.z);
            var shooterCartesianPos = shooterVectorPos.ToCartesian();

            var vectorDirection = (enemyCartesianPos - shooterCartesianPos).normalized;
            var direction = InputVectorToDirectionConverter.GetMovementDirectionFor(vectorDirection);
            _pathfinderAI.Rotate(direction);
            
            _projectileLauncher.Launch(enemyWorldPosition);
        }
    }
}
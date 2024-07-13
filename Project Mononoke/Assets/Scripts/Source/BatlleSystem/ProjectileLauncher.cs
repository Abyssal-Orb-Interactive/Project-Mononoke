using System;
using Base.Timers;
using UnityEngine;

namespace Source.BattleSystem
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab = null;
        [SerializeField] private float _speed = 0f;
        [SerializeField] private float _projectileSpawningOffset = 0f;
        
        
        private Projectile _launchedProjectile = null;
        private Vector3 _targetLaunched = Vector3.zero;

        public void Launch(Vector3 target)
        {
            if(_launchedProjectile != null) return;
            
            var direction = (target - transform.position).normalized;
            var spawnOffset = transform.position + direction * _projectileSpawningOffset;
            
            var projectile = Instantiate(_projectilePrefab, spawnOffset, Quaternion.identity);
            _targetLaunched = target;
            projectile.StartTimer();
            _launchedProjectile = projectile;
            _launchedProjectile.DisposeStarting += OnDispose;
        }

        private void OnDispose()
        {
            _launchedProjectile = null;
        }

        private void Update()
        {
            if(_launchedProjectile == null) return;
            _launchedProjectile.transform.position = Vector3.Lerp(_launchedProjectile.transform.position, _targetLaunched, _speed * Time.deltaTime);
            
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.BattleSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class DamageArea : MonoBehaviour, IDisposable
    {
        private HashSet<IDamageable> _damageablesInDamageArea = null;
        private IDamager _damageSource = null;

        public event Action TargetInZone = null;

        public void Initialize(IDamager damager)
        {
            _damageSource = damager;
            _damageablesInDamageArea = new HashSet<IDamageable>();
            SubscribeToDamageSourceAttacks();
        }

        private void SubscribeToDamageSourceAttacks()
        {
            if(_damageSource == null) return;
            _damageSource.Attack += OnAttack;
        }
        
        private void UnsubscribeFromDamageSourceAttacks()
        {
            if(_damageSource == null) return;
            _damageSource.Attack -= OnAttack;
        }

        private void OnAttack(IDamager obj)
        {
            ApplyAttackToAllDamagablesInArea();
        }

        private void ApplyAttackToAllDamagablesInArea()
        {
            var damageablesSnapshot = new HashSet<IDamageable>(_damageablesInDamageArea);
            
            foreach (var damageable in damageablesSnapshot)
            {
                damageable.TakeDamage(_damageSource);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_damageSource == null) return;
            if (!other.TryGetComponent<IDamageable>(out var target)) return;
            _damageablesInDamageArea.Add(target);
            TargetInZone?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(_damageSource == null) return;
            if (!other.TryGetComponent<IDamageable>(out var target)) return;

            if (_damageablesInDamageArea.Contains(target)) _damageablesInDamageArea.Remove(target);
        }

        private void OnDisable()
        {
            UnsubscribeFromDamageSourceAttacks();
        }

        private void OnEnable()
        {
            SubscribeToDamageSourceAttacks();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            UnsubscribeFromDamageSourceAttacks();
            TargetInZone = null;
            GC.SuppressFinalize(this);
        }
    }
}
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source.BattleSystem
{
    [RequireComponent(typeof(Damage2DTriggerArea))]
    public class DamageArea : MonoBehaviour, IDisposable
    {
        [SerializeField] private Damage2DTriggerArea damage2DTriggerArea = null;
        [SerializeField] private HashSet<IDamageable> _damageables = null;
        private IDamager _damager = null;

        public void Initialize(IDamager damager)
        {
            damage2DTriggerArea ??= GetComponent<Damage2DTriggerArea>();
            _damager ??= damager;
            _damageables = new HashSet<IDamageable>();
            SubscribeOnDamageAreaTriggers();
            SubscribeOnDamagerAttack();
        }

        private void SubscribeOnDamagerAttack()
        {
            _damager.Attack += OnAttack;
        }
        
        private void UnsubscribeOnDamagerAttack()
        {
            _damager.Attack -= OnAttack;
        }

        private void SubscribeOnDamageAreaTriggers()
        {
            damage2DTriggerArea.TargetEnteredInArea += OnTargetIn2DTriggerArea;
            damage2DTriggerArea.TargetExitFromArea += OnTargetExitFrom2DTriggerArea;
        }
        
        private void UnsubscribeOnDamageAreaTriggers()
        {
            damage2DTriggerArea.TargetEnteredInArea -= OnTargetIn2DTriggerArea;
            damage2DTriggerArea.TargetExitFromArea -= OnTargetExitFrom2DTriggerArea;
        }

        private void OnAttack(IDamager damager)
        {
            var damagablesSnapshot = new IDamageable[_damageables.Count];
            _damageables.CopyTo(damagablesSnapshot);
            foreach (var damageable in damagablesSnapshot)
            {
                damageable.TakeDamage(damager);
            }
        }

        private void OnTargetExitFrom2DTriggerArea(IDamageable enemy)
        {
            _damageables.Remove(enemy);
        }

        private void OnTargetIn2DTriggerArea(IDamageable enemy)
        {
            _damageables.Add(enemy);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            UnsubscribeOnDamagerAttack();
            UnsubscribeOnDamageAreaTriggers();
            damage2DTriggerArea?.Dispose();
            damage2DTriggerArea = null;
            _damager = null;
            _damageables.Clear();
            _damageables = null;
            GC.SuppressFinalize(this);
        }
    }
}
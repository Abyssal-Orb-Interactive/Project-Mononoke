using System;
using Base.Timers;
using Source.BattleSystem.Visual;
using UnityEngine;

namespace Source.BattleSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class Projectile : MonoBehaviour, IDamager, IDisposable
    {
        [SerializeField] private float _damage = 0f;
        [SerializeField] private Fractions _fraction = Fractions.Neutral;
        [SerializeField] private ShadowballAnimationDisposeTrigger _disposeTrigger = null;
        [SerializeField] private float _timeToLeave = 0f;

        private Timer _timer = null;
        
        public event Action<IDamager> Attack;
        public event Action DisposeStarting;

        public void StartTimer()
        {
            _timer = TimersFabric.Create(Timer.TimerType.ScaledFrame, _timeToLeave);
            _timer.TimerFinished += OnTimeToLeaveEnds;
            _timer.Start();
        }

        private void OnTimeToLeaveEnds()
        {
            OnHitSomething();
        }

        public float GetDamage()
        {
            return _damage;
        }

        public Fractions Fraction => _fraction;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<IDamageable>(out var damagable))
            {
                if (damagable.Fraction != _fraction)
                {
                    Attack?.Invoke(this);
                    damagable.TakeDamage(this);
                }
            }
            
            OnHitSomething();
        }

        public void OnHitSomething()
        {
            _timer.TimerFinished -= OnHitSomething;
            _timer.Stop();
            DisposeStarting?.Invoke();
            _disposeTrigger.DisposeEnds += OnDisposeEnds;
        }

        private void OnDisposeEnds()
        {
            Destroy(gameObject);
        }

        public void Dispose()
        {
            _timer.Stop();
            Attack = null;
            DisposeStarting = null;
            _disposeTrigger = null;
            _timer.Dispose();
            _timer = null;
            GC.SuppressFinalize(this);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
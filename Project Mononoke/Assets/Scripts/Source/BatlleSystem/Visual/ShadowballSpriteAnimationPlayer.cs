using System;
using UnityEngine;

namespace Source.BattleSystem.Visual
{
    [RequireComponent(typeof(Animator))]
    public class ShadowballSpriteAnimationPlayer : MonoBehaviour
    {
        private Animator _animator = null;
        [SerializeField] private Projectile _projectile = null;
        
        private static readonly int DisposeDesired = Animator.StringToHash("DisposeDesired");

        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }

        private void Start()
        {
            _animator ??= GetComponent<Animator>();
            _projectile.DisposeStarting += OnDisposing;
        }

        private void OnDisposing()
        {
            _animator.SetBool(DisposeDesired, true);
        }

        private void OnEndDisposing()
        {
            _animator.SetBool(DisposeDesired, false);
        }
    }
}
using System;
using UnityEngine;

namespace Source.BuildingModule.Buildings.Visual
{
    [RequireComponent(typeof(Animator))]
    public class BarrelSpriteAnimationPlayer : MonoBehaviour, IDisposable
    {
        [SerializeField] private Animator _animator = null;
        [SerializeField] private Container _container = null;
        private static readonly int OpenDesired = Animator.StringToHash("Open Desired");
        

        public void Initialize()
        {
            _animator ??= GetComponent<Animator>();
            _container ??= transform.parent.GetComponent<Container>();
            StartListeningContainerEvents();
        }

        private void StartListeningContainerEvents()
        {
            _container.ContainerOpened += OnContainerOpened;
            _container.ContainerClosed += OnContainerClosed;
        }

        private void OnContainerClosed()
        {
            _animator.SetBool(OpenDesired, false);
        }

        private void OnContainerOpened()
        {
            _animator.SetBool(OpenDesired, true);
        }
        
        private void StopListeningContainerEvents()
        {
            _container.ContainerOpened -= OnContainerOpened;
            _container.ContainerClosed -= OnContainerClosed;
        }

        public void Dispose()
        {
            StopListeningContainerEvents();
            _container = null;
            _animator.SetBool(OpenDesired, false);
            _animator = null;
            GC.SuppressFinalize(this);
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
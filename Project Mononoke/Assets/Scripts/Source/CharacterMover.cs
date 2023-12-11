using System;
using Source.Input;
using UnityEngine;

namespace Source
{
    public class CharacterMover : MonoBehaviour, IDisposable
    {
        [SerializeField] private InputManager _inputManager;
        private Vector2 _moveDirection = Vector2.zero;

        private void Start()
        {
            if(_inputManager == null) return;
            _inputManager.Initialize();
            StartInputHandling();
        }
        
        private void OnMovementInputChange(object sender, InputManager.OnMovementInputChangedEventArgs args)
        {
            _moveDirection = args.MovementVector;
        }

        private void StartInputHandling()
        {
            _inputManager.AddMovementInputChangedHandler(OnMovementInputChange);
        }
        
        private void StopInputHandling()
        {
            _inputManager.RemoveMovementInputChangedHandler(OnMovementInputChange);
        }
        
        private void OnEnable()
        {
            if(_inputManager == null) return;
            StartInputHandling();
        }

        private void OnDisable()
        {
            if(_inputManager == null) return;
            StopInputHandling();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            StopInputHandling();
            _inputManager = null;
        }
    }
}
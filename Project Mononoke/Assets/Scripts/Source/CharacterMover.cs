using System;
using Source.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source
{
    public class CharacterMover : MonoBehaviour, IDisposable
    {
        [FormerlySerializedAs("_inputManager")] [SerializeField] private InputHandler inputHandler;
        [SerializeField] private float _speed = 10f;
        private Transform _transform = null;
        private Vector2 _moveDirection = Vector2.zero;

        private void Start()
        {
            if(inputHandler == null) return;
            _transform = gameObject.transform;
            inputHandler.Initialize(new TestActions());
            StartInputHandling();
        }

        private void Update()
        {
            _transform.Translate(_moveDirection * (_speed * Time.deltaTime));
        }

        private void OnMovementInputChange(object sender, InputHandler.InputActionEventArgs args)
        {
            if(args.Action != InputHandler.InputActionEventArgs.ActionType.Movement) return;
            _moveDirection = (Vector2) args.ActionData;
           
        }

        private void StartInputHandling()
        {
            inputHandler.AddInputChangedHandler(OnMovementInputChange);
        }
        
        private void StopInputHandling()
        {
            inputHandler.RemoveInputChangedHandler(OnMovementInputChange);
        }
        
        private void OnEnable()
        {
            if(inputHandler == null) return;
            StartInputHandling();
        }

        private void OnDisable()
        {
            if(inputHandler == null) return;
            StopInputHandling();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            StopInputHandling();
            inputHandler = null;
        }
    }
}
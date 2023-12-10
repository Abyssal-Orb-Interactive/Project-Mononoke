using System;
using UnityEngine;

namespace Source.Input
{
    public delegate void InputAction();
    public class InputManager : MonoBehaviour, IDisposable
    {
        private TestActions _input;
        private event InputAction OnPlayerClick;
        private event InputAction OnPlayerExit;

        public void Initialize(TestActions input)
        {
            _input = input;
            StartInputHandling();
        }

        public void AddClickAction(InputAction action)
        {
            OnPlayerClick += action;
        }
        public void RemoveClickAction(InputAction action)
        {
            OnPlayerClick -= action;
        }
        public void AddExitAction(InputAction action)
        {
            OnPlayerExit += action;
        }
        public void RemoveExitAction(InputAction action)
        {
            OnPlayerExit -= action;
        }

        private void OnEnable()
        {
            StartInputHandling();
        }

        private void OnDisable()
        {
            StopInputHandling();
        }

        private void OnClickPerformed(UnityEngine.InputSystem.InputAction.CallbackContext player)
        {
            OnPlayerClick?.Invoke();
        }
        
        private void OnExitPerformed(UnityEngine.InputSystem.InputAction.CallbackContext player)
        {
            OnPlayerExit?.Invoke();
        }

        private void SubscribeToClick()
        {
            _input.TestMap.Click.performed += OnClickPerformed;
        }
        private void UnsubscribeToClick()
        {
            _input.TestMap.Click.performed -= OnClickPerformed;
        }
        private void SubscribeToExit()
        {
            _input.TestMap.Exit.performed += OnExitPerformed;
        }
        private void UnsubscribeToExit()
        {
            _input.TestMap.Exit.performed -= OnExitPerformed;
        }

        private void StartInputHandling()
        {
            _input.Enable();
            SubscribeToClick();
            SubscribeToExit();
        }
        
        private void StopInputHandling()
        {
            _input.Disable();
            UnsubscribeToClick();
            UnsubscribeToExit();
            OnPlayerClick = null;
            OnPlayerExit = null;
        }
        
        public void Dispose()
        {
            StopInputHandling();
            _input?.Dispose();
            _input = null;
            GC.SuppressFinalize(this);
        }
    }
}
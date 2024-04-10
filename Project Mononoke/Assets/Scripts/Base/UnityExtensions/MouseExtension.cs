using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.UnityExtensions
{
    public static class MouseExtension
    { 
        private static Camera MainCamera => Camera.main;
        
        public static Vector3 GetMouseWorldPosition(this Mouse mouse, Camera worldCamera = null)
        {
            worldCamera ??= MainCamera;
            
            Vector2 mousePosition = mouse.position.ReadValue();
            var screenPosition = new Vector3(mousePosition.x, mousePosition.y, worldCamera.nearClipPlane);
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);

            return worldPosition;
        }
    }
}
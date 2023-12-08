using UnityEngine;
using UnityEngine.InputSystem;

namespace Base.Utils
{
    public static class MouseUtils
    {
        private static Vector3 MouseScreenPosition => Input.mousePosition;
        private static Camera MainCamera => Camera.main;

        public static Vector3 GetMouseWorldPosWithoutZ(Vector3? screenPosition = null, Camera worldCamera = null)
        {
            var position = GetMouseWorldPosition(screenPosition, worldCamera);
            position.z = 0f;
            return position;
        }
        
        public static Vector3 GetMouseWorldPosition(Vector3? screenPosition = null, Camera worldCamera = null)
        {
            worldCamera ??= MainCamera;
            screenPosition ??= MouseScreenPosition;
            
            var worldPosition = worldCamera.ScreenToWorldPoint((Vector3)screenPosition);
            return worldPosition;
        }
        
        public static Vector3 GetMouseWorldPosWithoutZUsingNewInputSystem(Vector3? screenPosition = null, Camera worldCamera = null)
        {
            var position = GetMouseWorldPosition(screenPosition, worldCamera);
            position.z = 0f;
            return position;
        }
        
        public static Vector3 GetMouseWorldPositionUsingNewInputSystem(Vector3? screenPosition = null, Camera worldCamera = null)
        {
            worldCamera ??= MainCamera;
            var mousePosition = Mouse.current.position.ReadValue();
            screenPosition ??= new Vector3(mousePosition.x, mousePosition.y, worldCamera.nearClipPlane);
            
            return GetMouseWorldPosition(screenPosition, worldCamera);
        }
    }
}
using UnityEngine;

namespace Base.Utils
{
    public static class MouseUtils
    {
        private static Vector3 MouseScreenPosition => Input.mousePosition;
        private static Camera MainCamera => Camera.main;

        public static Vector3 GetMouseWorldPosWithoutZ(Vector3? screenPosition = null, Camera worldCamera = null)
        {
            var position = GetMouseWorldPosition();
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
    }
}
using Base.UnityExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Source.UI
{
    public class MousePointerMover : MonoBehaviour
    {
        private GameObject _mousePointer = null;
        private Mouse _cursor = null;

        private void OnValidate()
        {
            _mousePointer ??= gameObject;
            _cursor ??= Mouse.current;
        }

        public void Update()
        {
            _mousePointer.transform.position = _cursor.GetMouseWorldPosition();
        }
    }
}

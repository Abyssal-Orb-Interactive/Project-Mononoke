using Base.Utils;
using UnityEngine;

namespace Source
{
    public class MousePointerMover : MonoBehaviour
    {
        private GameObject _mousePointer = null;

        public void Update()
        {
            _mousePointer ??= gameObject;
            _mousePointer.transform.position = MouseUtils.GetMouseWorldPosWithoutZUsingNewInputSystem();
        }
    }
}

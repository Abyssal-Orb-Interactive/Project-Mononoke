using Source.InventoryModule.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using static Source.InventoryModule.InventoryPresenter;

namespace Source.UI
{
    public class MousePointerMover : MonoBehaviour
    {
        private Canvas _canvas = null;
        private Mouse _cursor = null;
        [SerializeField] private ItemUIElement _UIElement = null;

        private void OnValidate()
        {
            _canvas = gameObject.transform.root.GetComponent<Canvas>();
            _cursor ??= Mouse.current;
            //_UIElement = GetComponentInChildren<ItemUIElement>();
            Toggle(false);
        }

        public void SetData(StackDataForUI stack)
        {
            _UIElement.InitializeWith(stack);
        }

        public void Update()
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle
            (
                (RectTransform)_canvas.transform,
                _cursor.position.ReadValue(),
                _canvas.worldCamera,
                out Vector2 position
            );

            transform.position = _canvas.transform.TransformPoint(position);
        }

        public void Toggle(bool signal)
        {
            _UIElement?.gameObject.SetActive(signal);
        }
    }
}

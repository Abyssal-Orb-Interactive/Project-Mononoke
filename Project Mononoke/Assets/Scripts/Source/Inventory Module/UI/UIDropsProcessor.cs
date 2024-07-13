using Base.Input;
using Source.Character.Movement;
using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.InventoryModule.UI
{
    public class UIDropsProcessor
    {
        private readonly RectTransform _otherViewRectTransform = null;
        private readonly Inventory _otherInventory = null;
        private readonly ItemDropper _dropper = null;

        public UIDropsProcessor(InventoryPresenter presenter, RectTransform otherViewRectTransform, Inventory otherOtherInventory, ItemDropper dropper)
        {
            _otherInventory = otherOtherInventory;
            _otherViewRectTransform = otherViewRectTransform;
            _dropper = dropper;

            presenter.ItemDropped += OnStackDropped;
        }

        private void OnStackDropped(Item item)
        {
            if (item == null) return;

            Vector2 mousePosition = Input.mousePosition;
            
            var canvas = _otherViewRectTransform.GetComponentInParent<Canvas>();
            var renderMode = canvas.renderMode;
            var canvasCamera =
                renderMode is RenderMode.ScreenSpaceCamera or RenderMode.WorldSpace
                    ? canvas.worldCamera
                    : null;

            if (RectTransformUtility.RectangleContainsScreenPoint(_otherViewRectTransform, mousePosition, canvasCamera))
            {
                _otherInventory.TryAddItem(item);
            }
            else
            {
                CreateItemView(item);
            }
        }

        private void CreateItemView(Item item)
        {
            _dropper.Drop(item);
        }
    }
}
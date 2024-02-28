using System;
using Source.ItemsModule;
using UnityEngine;

namespace Source.PickUpModule
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HandlingItemVisualizer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer = null;
        private Manipulator _manipulator = null;

        private void OnValidate()
        {
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }

        public void InitializeWith(Manipulator manipulator)
        {
            _manipulator = manipulator;
            _manipulator.InManipulatorItemChanged += ChangeSprite;
        }

        private void ChangeSprite(Item<ItemData> item)
        {
            if (!item.Database.TryGetItemDataBy(item.ID, out var data)) return;
            _spriteRenderer.sprite = data.UIData.Icon;
        }
    }
}
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

        private void ChangeSprite(Item item)
        {
            if (item == null)
            {
                _spriteRenderer.sprite = null;
                return;
            }

            var data = item.Data;
            if (data == null) return;
            _spriteRenderer.sprite = data.UIData.Icon;
        }
    }
}
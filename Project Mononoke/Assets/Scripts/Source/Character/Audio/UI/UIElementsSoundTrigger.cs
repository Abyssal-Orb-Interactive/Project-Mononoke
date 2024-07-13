using System;
using Source.InventoryModule.UI;
using UnityEngine;

namespace Source.Character.Audio
{
    [RequireComponent(typeof(AudioPlayer))]
    [RequireComponent(typeof(ItemUIElement))]
    public class UIElementsSoundTrigger : MonoBehaviour
    {
        private AudioPlayer _player = null;
        private ItemUIElement _uiElement = null;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _player ??= GetComponent<AudioPlayer>();
            _uiElement ??= GetComponent<ItemUIElement>();
            _uiElement.OnItemLeftClicked += OnLeftClick;
            _uiElement.OnItemRightClicked += OnRightClick;
            _uiElement.OnItemBeginDrag += OnBeginDrag;
            _uiElement.OnItemEndDrag += OnEndDrag;
        }

        private void OnEndDrag(ItemUIElement obj)
        {
            _player.PlaySound(AudioTypes.UIItemEndDrag);
        }

        private void OnBeginDrag(ItemUIElement obj)
        {
            _player.PlaySound(AudioTypes.UIItemBeginDrag);
        }

        private void OnRightClick(ItemUIElement obj)
        {
            _player.PlaySound(AudioTypes.UIItemMenuOpens);
        }

        private void OnLeftClick(ItemUIElement obj)
        {
            _player.PlaySound(AudioTypes.UIItemChoose);
        }

    }
}
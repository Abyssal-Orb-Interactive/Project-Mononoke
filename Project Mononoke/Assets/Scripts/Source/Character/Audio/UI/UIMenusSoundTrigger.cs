using System;
using Source.InventoryModule.UI;
using UnityEngine;

namespace Source.Character.Audio
{
    [RequireComponent(typeof(InventoryMenuSwitcher))]
    public class UIMenusSoundTrigger : MonoBehaviour
    { 
        [SerializeField] private AudioPlayer _audioPlayer = null;
        [SerializeField] private InventoryMenuSwitcher _menuSwitcher = null;

        private void Start()
        {
            _menuSwitcher.MenuSwitched += PlayUIMenuOpensSound;
        }

        private void OnValidate()
        {
            _audioPlayer ??= transform.parent.GetComponentInChildren<AudioPlayer>();
            _menuSwitcher = transform.GetComponent<InventoryMenuSwitcher>();
        }

        private void PlayUIMenuOpensSound()
        {
            _audioPlayer.PlaySound(AudioTypes.UIMenuOpens, 1f);
        }

        public void PlayUIButtonClickSound()
        {
            _audioPlayer.PlaySound(AudioTypes.UIButtonClick);
        }
    }
}
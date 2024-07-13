using Source.ItemsModule;
using Source.PickUpModule;
using UnityEngine;

namespace Source.Character.Audio
{
    [RequireComponent(typeof(AudioPlayer))]
    [RequireComponent(typeof(PickUpper))]
    public class PickUpperSoundTrigger : MonoBehaviour
    {
        private AudioPlayer _player = null;
        private PickUpper _pickUpper = null;
        
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _player ??= GetComponent<AudioPlayer>();
            _pickUpper ??= GetComponent<PickUpper>();
            _pickUpper.ItemEquipped += OnItemEquipped;
            _pickUpper.ItemStashed += OnStashItem;
            _pickUpper.ItemPickUpped += OnItemPickUp;
        }

        private void OnItemPickUp(Item obj)
        {
            _player.PlaySound(AudioTypes.PickUpItem, 2f);
        }

        private void OnStashItem()
        {
            _player.PlaySound(AudioTypes.StashItem);
        }

        private void OnItemEquipped()
        {
            _player.PlaySound(AudioTypes.EquipItem);
        }
    }
}